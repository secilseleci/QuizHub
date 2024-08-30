using Microsoft.AspNetCore.Mvc;

namespace QuizHubPresentation.Components
{
    public class QuizFilterMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
