using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models;
using Services.Contracts;
using System.Security.Claims;

namespace QuizHubPresentation.Components
{
    public class ScoreViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ScoreViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            var userId = (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcının quiz verilerini yeni sisteme göre alıyoruz
            var userQuizInfoResult = _manager.UserQuizInfoService.GetUserQuizInfoByUserId(userId, trackChanges: false).Result;

            if (!userQuizInfoResult.IsSuccess || userQuizInfoResult.Data == null)
            {
                // Eğer veri alınamıyorsa hata görüntülenir veya default bir değer döneriz
                return View("Default", new PerformanceViewModel
                {
                    UserName = userName,
                    UserId = userId,
                    AverageScore = 0
                });
            }

            // Kullanıcıya ait quiz bilgilerini aldıktan sonra ortalama hesaplanır
            var performanceViewModel = new PerformanceViewModel
            {
                UserName = userName,
                UserId = userId,
                AverageScore = userQuizInfoResult.Data.Any()
                    ? Math.Round(userQuizInfoResult.Data.Average(q => q.Score), 2)
                    : 0
            };

            return View("Default", performanceViewModel);
        }
    }
}
