using System.Diagnostics;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    /// <summary>
    ///   Controller responsible for handling the main pages of the application (Home, Privacy, Error).
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        ///   Constructor for HomeController.
        /// </summary>
        /// <param name="logger">Provides logging capabilities.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///   Displays the main landing page of the application (Home page).
        /// </summary>
        /// <returns>The Home page view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///   Displays the Privacy page.
        /// </summary>
        /// <returns>The Privacy page view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        ///   Handles displaying error information.
        /// </summary>
        /// <returns>The Error page view, with error details.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Disables caching to ensure fresh error info
        public IActionResult Error()
        {
            // Creates an ErrorViewModel to pass error details to the view
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            // Activity.Current?.Id gets the current activity's ID (useful for tracing)
            // HttpContext.TraceIdentifier provides a unique ID for the current request
        }
    }
}