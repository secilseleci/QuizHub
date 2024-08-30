using Entities.Dtos;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models;
using Services.Contracts;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class QuizController : Controller
    {
        private readonly IServiceManager _manager;

        public QuizController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index([FromQuery] QuizRequestParameters q)
        {
            ViewData["Title"] = "Quizzes";

            var quizzes = _manager.QuizService.GetAllQuizzesWithDetails(q);
            var pagination = new Pagination()
            {
                CurrenPage = q.PageNumber,
                ItemsPerPage = q.PageSize,
                TotalItems = _manager.QuizService.GetAllQuizzes(false).Count()
            };
            return View(new QuizListViewModel()
            {
                Quizzes = quizzes,
                Pagination = pagination
            });
        }

        public IActionResult Create()
        {
            TempData["info"] = "Please fill the form.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] QuizDtoForInsertion quizDto)
        {
            if (ModelState.IsValid)
            {
                _manager.QuizService.CreateQuiz(quizDto);
                TempData["success"] = $"{quizDto.Title} has been created.";
                return RedirectToAction("Index");
            }
            return View();
        }
         


        public IActionResult Update([FromRoute(Name = "id")] int id)
        {
            var model = _manager.QuizService.GetOneQuizForUpdate(id, false);
            ViewData["Title"] = model?.Title;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] QuizDtoForUpdate quizDto)
        {
            if (ModelState.IsValid)
            {
                _manager.QuizService.UpdateOneQuiz(quizDto);
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Delete([FromRoute(Name = "id")] int id)
        {
            _manager.QuizService.DeleteOneQuiz(id);
            TempData["danger"] = "The quiz has been removed.";
            return RedirectToAction("Index");
        }
    }
}