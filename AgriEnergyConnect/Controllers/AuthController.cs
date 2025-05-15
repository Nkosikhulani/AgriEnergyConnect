using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using AgriEnergyConnect.Data;

namespace AgriEnergyConnect.Controllers
{
    /// <summary>
    ///   Controller responsible for user authentication (login, registration, logout).
    /// </summary>
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        ///   Constructor for AuthController.
        /// </summary>
        /// <param name="userManager">Provides APIs for managing users in the Identity system.</param>
        /// <param name="signInManager">Provides APIs for user sign-in.</param>
        /// <param name="dbContext">The application's database context.</param>
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        /// <summary>
        ///   Displays the login page (HTTP GET).
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        ///   Handles user login (HTTP POST).
        /// </summary>
        /// <param name="model">The LoginViewModel containing user login credentials.</param>
        /// <returns>
        ///   Redirects to the Home page on successful login, otherwise returns the login view with an error message.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents cross-site request forgery attacks
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) // Checks if the model (form data) is valid based on validation rules
            {
                // Attempts to sign in the user using the provided email and password
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to the Home controller's Index action
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt."); // Adds an error to the model state
                    return View(model); // Return the login view with the error message
                }
            }
            return View(model); // If the model is not valid, return the login view with validation errors
        }

        /// <summary>
        ///   Displays the registration page (HTTP GET).
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        ///   Handles user registration (HTTP POST).
        /// </summary>
        /// <param name="model">The RegisterViewModel containing user registration data.</param>
        /// <returns>
        ///   Redirects to the Home page on successful registration, otherwise returns the registration view with errors.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents cross-site request forgery attacks
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Checks if the model (form data) is valid
            {
                // Creates a new User object with data from the registration form
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Role = model.Role,
                    FarmName = model.FarmName,
                    Department = model.Department,
                    FirstName = model.FirstName,  // Include FirstName
                    LastName = model.LastName,    // Include LastName
                    Location = model.Location,      // Include Location
                    EmployeeId = model.EmployeeId   // Include EmployeeId
                };

                // Attempts to create the user in the Identity system
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Signs the user in after successful registration
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // Redirect to the Home controller's Index action
                }
                else
                {
                    // Adds any errors from the Identity system to the model state
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model); // Return the registration view with the errors
                }
            }
            return View(model); // If the model is not valid, return the registration view with validation errors
        }

        /// <summary>
        ///   Handles user logout (HTTP POST).
        /// </summary>
        /// <returns>Redirects to the Home page after successful logout.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents cross-site request forgery attacks
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Signs the user out
            return RedirectToAction("Index", "Home"); // Redirect to the Home controller's Index action
        }
    }
}