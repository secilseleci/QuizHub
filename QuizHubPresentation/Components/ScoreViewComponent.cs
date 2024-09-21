using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models.Profile;
using Services.Contracts;
using System.Security.Claims; // ClaimTypes için gerekli

namespace QuizHubPresentation.Components
{
    public class ScoreViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        // Constructor: UserId'yi burada alıyoruz
        public ScoreViewComponent(IServiceManager manager)
        {
            _manager = manager;

           
        }

        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            // Kullanıcı ID'sini doğru şekilde alıyoruz
            var userId = (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcının quiz verilerini alıyoruz
            var userQuizInfo = _manager.UserQuizInfoService.GetUserQuizInfoByUserId(userId, false).ToList();


            var performanceViewModel = new PerformanceViewModel
            {
                UserName=userName,
                UserId = userId,  
                AverageScore = userQuizInfo.Any() ? Math.Round(userQuizInfo.Average(q => q.Score), 2) : 0
            };

            return View("Default", performanceViewModel);
        }
    }
}
