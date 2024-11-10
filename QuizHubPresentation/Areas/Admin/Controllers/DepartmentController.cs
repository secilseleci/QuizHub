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
                TempData["ErrorMessage"] = "Departmanlar y�klenemedi.";
                return View("Error"); // Hata sayfas�na y�nlendiriliyor.
            }

            return View(departmentsResult.Data); // Ba�ar�l�ysa View'e departmanlar� g�nder.
        }

        [HttpGet]
        public async Task<IActionResult> Quizzes(int departmentId)
        {
            var departmentResult = await _serviceManager.DepartmentService.GetDepartmentWithQuizzes(departmentId, trackChanges: false);

            if (!departmentResult.IsSuccess)
            {
                return NotFound("Departman veya atanan quizler bulunamad�.");
            }

            return PartialView("_QuizList", departmentResult.Data.Quizzes); // Quiz verileriyle partial view d�nd�r.
        }
    }
}
