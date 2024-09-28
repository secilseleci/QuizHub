using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {
        private readonly IServiceManager _manager;

        public DepartmentController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var departments = _manager.DepartmentService.GetAllDepartments(trackChanges: false);
            return View(departments);
        }

        [HttpGet]
        public IActionResult Quizzes(int departmentId)
        {
            // Departmana atanmýþ quizleri alýyoruz
            var department = _manager.DepartmentService.GetDepartmentWithQuizzes(departmentId, trackChanges: false);

            if (department == null)
            {
                return NotFound();
            }

            return PartialView("_QuizList", department.Quizzes);

        }

    }
}