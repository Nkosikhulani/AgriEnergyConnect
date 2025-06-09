using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    using AgriEnergyConnect.Data;
    using AgriEnergyConnect.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    //[Authorize(Roles = "Employee")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public IActionResult Add()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(Farmer farmer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Farmers.Add(farmer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(farmer);
        //}

        // GET: /Farmer
        // Displays a list of products.
        // You might want to filter this to only show products by the logged-in farmer.
        public async Task<IActionResult> Index()
        {
            // Option 1: Show all products (useful for general Browse)
            var farmers = await _context.Farmers.Include(p => p.User).ToListAsync();
            return View(farmers);

            // Option 2: Show only products owned by the currently logged-in farmer
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // if (!string.IsNullOrEmpty(userId))
            // {
            //     var farmerProducts = await _context.Products
            //                                         .Include(p => p.User)
            //                                         .Where(p => p.UserId == userId)
            //                                         .ToListAsync();
            //     return View(farmerProducts);
            // }
            // // If no user is logged in, perhaps show an empty list or redirect to login
            // return View(new List<Product>());
        }

        // GET: /Product/Details/5
        // Displays the details of a specific product.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(p => p.User) // Eagerly load the associated User data
                .FirstOrDefaultAsync(m => m.Id == id);

            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: /Product/Add
        // Displays the form to add a new product.
        [Authorize] // Only authenticated users (farmers) can add products
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Only authenticated users (farmers) can add products
        public async Task<IActionResult> Add([Bind("FullName,Email,Location,DateJoined")] Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                // Get current user's ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    // This scenario should be rare with [Authorize], but good for robustness
                    ModelState.AddModelError("", "User is not logged in. Please log in to add products.");
                    return View(farmer);
                }

                farmer.UserId = userId; // Assign the logged-in user's ID to the product

                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync(); // Commit changes to the database
                return RedirectToAction(nameof(Index)); // Redirect to the product list
            }

            // If ModelState is not valid, or if userId is null/empty (fallback), re-display the form with validation errors
            return View(farmer);
        }

        // GET: /Product/Edit/5
        // Displays the form to edit an existing product.
        [Authorize] // Only authenticated users can edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }

            // IMPORTANT: Ownership check - ensure the logged-in user owns this product
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (farmer.UserId != userId)
            {
                // Prevent unauthorized access/editing
                return Forbid(); // Returns a 403 Forbidden status
                // Or: return Unauthorized(); // Returns a 401 Unauthorized status
                // Or: return RedirectToPage("/AccessDenied"); // Redirect to a custom access denied page
            }

            return View(farmer);
        }

        // POST: /Product/Edit/5
        // Handles the submission of the edit form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Only authenticated users can edit
        public async Task<IActionResult> Edit(int id, [Bind("FullName,Email,Location,DateJoined")] Farmer farmer)
        {
            if (id != farmer.Id)
            {
                return NotFound();
            }

            // Re-check ownership before updating to prevent tampering via form data
            // Fetch the existing product from the database, but don't track it yet.
            var existinFarmer = await _context.Farmers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (existinFarmer == null || existinFarmer.UserId != userId)
            {
                // Product not found or user doesn't own it
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure the UserId is not tampered with from the form.
                    // Use the original owner's ID from the fetched existing product.
                    farmer.UserId = existinFarmer.UserId;

                    _context.Update(farmer); // Mark the product as modified
                    await _context.SaveChangesAsync(); // Commit changes

                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency conflicts (e.g., product deleted by another user)
                    if (!_context.Farmers.Any(e => e.Id == farmer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw if it's another type of concurrency issue
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to the product list after successful edit
            }
            return View(farmer); // Re-display form if model state is invalid
        }

        // GET: /Product/Delete/5
        // Displays the confirmation page for deleting a product.
        [Authorize] // Only authenticated users can delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(p => p.User) // Eagerly load the associated User data for display on confirmation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            // IMPORTANT: Ownership check - ensure the logged-in user owns this product
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (farmer.UserId != userId)
            {
                return Forbid(); // Prevent unauthorized deletion
            }

            return View(farmer);
        }

        // POST: /Product/Delete/5
        // Handles the actual deletion of the product after confirmation.
        [HttpPost, ActionName("Delete")] // ActionName specifies this POST method maps to "Delete" action
        [ValidateAntiForgeryToken]
        [Authorize] // Only authenticated users can delete
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // IMPORTANT: Ownership check immediately before deletion
            if (farmer == null || farmer.UserId != userId)
            {
                return Forbid(); // Product not found or user doesn't own it
            }

            _context.Farmers.Remove(farmer); // Mark product for removal
            await _context.SaveChangesAsync(); // Commit deletion to the database
            return RedirectToAction(nameof(Index)); // Redirect to the product list
        }
    }

}
