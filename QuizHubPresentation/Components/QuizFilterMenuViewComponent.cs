using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

namespace QuizHubPresentation.Components
{
    public class QuizFilterMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public QuizFilterMenuViewComponent(IServiceManager serviceManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _serviceManager = serviceManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var quizzesResult = await _serviceManager.QuizService.GetQuizzesWithDepartmentsAsync(trackChanges: false);

            if (!quizzesResult.IsSuccess || quizzesResult.Data == null)
            {
                return Content("No quizzes available.");
            }

            var quizzes = quizzesResult.Data.ToList();

            // Kullanıcı giriş yapmamışsa (Anonymous user)
            if (!User.Identity.IsAuthenticated)
            {
                quizzes = quizzes.Where(q => q.ShowCase && q.Departments.All(d => d.DepartmentId == null)).ToList();
                return View("GuestQuizCard", quizzes); // Guest kartları
            }

            // Kullanıcı giriş yapmışsa rollerine göre ayır
            var claimsPrincipal = User as ClaimsPrincipal;
            var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier); // UserId'yi alıyoruz
            var user = await _userManager.FindByIdAsync(userId);  // ApplicationUser kullanıyoruz

            if (user == null)
            {
                return Content("User not found"); // Eğer kullanıcı bulunamazsa
            }

            // Kullanıcının rolünü alalım
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault(); // İlk rolü alalım (Admin, Editor, User)

            // Admin için filtreleme yok
            if (userRole == "Admin")
            {
                return View("AdminQuizCard", quizzes); // Tüm quizler Admin için gösterilir
            }

            // Editor veya User için departmentId'ye göre filtreleme
            var departmentId = user.DepartmentId;

            if (departmentId != null)
            {
                quizzes = quizzes.Where(q => q.ShowCase && q.Departments != null && q.Departments.Any(d => d.DepartmentId == departmentId)).ToList();
            }

            return View("UserQuizCard", quizzes); // User veya Editor kartları
        }
    }
}
