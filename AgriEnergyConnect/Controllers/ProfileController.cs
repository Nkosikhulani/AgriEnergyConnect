using Microsoft.AspNetCore.Authorization; // For [Authorize] attribute
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using AgriEnergyConnect.Models; // Assuming your custom User model is here

namespace AgriEnergyConnect.Controllers
{
    // [Authorize] attribute ensures only logged-in users can access this controller's actions
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Profile
        public async Task<IActionResult> Index()
        {
            // Get the ID of the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                // This should ideally not happen with [Authorize], but good for defensive coding
                return RedirectToAction("Login", "Auth");
            }

            // Retrieve the user object from UserManager
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found in database, which is an unusual state if authenticated
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // Pass the user object to the view
            return View(user);
        }

        // You might want to add other actions like EditProfile, ChangePassword etc.
        // For example, an Edit action:
        // [HttpGet]
        // public async Task<IActionResult> Edit()
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     var user = await _userManager.FindByIdAsync(userId);
        //     if (user == null)
        //     {
        //         return NotFound($"Unable to load user with ID '{userId}'.");
        //     }
        //     // You might want to map User to a ProfileViewModel for editing specific fields
        //     var model = new ProfileViewModel {
        //         FirstName = user.FirstName,
        //         LastName = user.LastName,
        //         Email = user.Email,
        //         FarmName = user.FarmName,
        //         Location = user.Location,
        //         Department = user.Department,
        //         EmployeeId = user.EmployeeId
        //     };
        //     return View(model);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(ProfileViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }

        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     var user = await _userManager.FindByIdAsync(userId);

        //     if (user == null)
        //     {
        //         return NotFound($"Unable to load user with ID '{userId}'.");
        //     }

        //     // Update user properties
        //     user.FirstName = model.FirstName;
        //     user.LastName = model.LastName;
        //     user.Email = model.Email; // Be careful if changing email affects UserName
        //     // user.UserName = model.Email; // Consider if UserName should also change
        //     user.FarmName = model.FarmName;
        //     user.Location = model.Location;
        //     user.Department = model.Department;
        //     user.EmployeeId = model.EmployeeId;

        //     var result = await _userManager.UpdateAsync(user);

        //     if (result.Succeeded)
        //     {
        //         TempData["SuccessMessage"] = "Profile updated successfully!";
        //         return RedirectToAction(nameof(Index));
        //     }

        //     foreach (var error in result.Errors)
        //     {
        //         ModelState.AddModelError(string.Empty, error.Description);
        //     }
        //     return View(model);
        // }
    }
}