using Microsoft.AspNetCore.Mvc;

namespace QuizHubPresentation.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("PageNotFound");
            }
            else if (statusCode == 403)
            {
                return View("AccessDenied");
            }
            else if (statusCode == 500)
            {
                return View("InternalServerError");
            }
            return View("GeneralError");
        }
    }

}
