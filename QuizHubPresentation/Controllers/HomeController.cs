using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
  
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TempData["info"] = $"Welcome back, {DateTime.Now.ToShortTimeString()}";
            return View();
        }
        public IActionResult Details()
        {
             
            return View();
        }

        public IActionResult Start()
        {
             return View();
        }

    }
}