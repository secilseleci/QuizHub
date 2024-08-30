using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizHubPresentation.Components
{
    public class QuizSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;
        public QuizSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }
        public string Invoke()
        {
            return _manager.QuizService.GetAllQuizzes(false).Count().ToString();
        }
    }
}
