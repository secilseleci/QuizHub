using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public DepartmentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index()
        {
            var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(trackChanges: false);

            if (!departmentsResult.IsSuccess)
            {
                TempData["ErrorMessage"] = "Departmanlar yüklenemedi.";
                return View("Error"); // Hata sayfasýna yönlendiriliyor.
            }

            return View(departmentsResult.Data); // Baþarýlýysa View'e departmanlarý gönder.
        }

        [HttpGet]
        public async Task<IActionResult> Quizzes(int departmentId)
        {
            var departmentResult = await _serviceManager.DepartmentService.GetDepartmentWithQuizzes(departmentId, trackChanges: false);

            if (!departmentResult.IsSuccess)
            {
                return NotFound("Departman veya atanan quizler bulunamadý.");
            }

            return PartialView("_QuizList", departmentResult.Data.Quizzes); // Quiz verileriyle partial view döndür.
        }
    }
}
