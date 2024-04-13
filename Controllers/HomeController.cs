using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EatEvals.Areas.Identity.Data;
using EatEvals.Models;
using System.Diagnostics;

namespace EatEvals.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<RestaurantReviewUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<RestaurantReviewUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        [Authorize]
        public IActionResult IndexAdmin()
        {
            return RedirectToAction("Index");

        }

        public IActionResult Index()
        {
            ViewBag.isAdmin = User.Identity.IsAuthenticated && User?.Identity.Name == "ConestogaCollege.123@gmail.com";
            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}