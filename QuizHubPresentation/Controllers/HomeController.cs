using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizHubPresentation.Controllers
{
    public class HomeController : Controller
    {
  
        public IActionResult Index()
        {
            ViewData["Title"] = "Welcome";
            return View();
        }



    }
}
