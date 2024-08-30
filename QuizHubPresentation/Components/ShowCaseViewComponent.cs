using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizHubPresentation.Components
{
    public class ShowCaseViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ShowCaseViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public IViewComponentResult Invoke(string page = "default")
        {
            var quizzes = _manager.QuizService.GetShowCaseQuizzes(false);
            return page.Equals("default")
                ? View(quizzes)
                : View("List", quizzes);
        }
    }
}
 
