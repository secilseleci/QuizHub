using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models;
using Services.Contracts;

namespace QuizHub.Controllers
{
    public class QuizController : Controller
    {
        private readonly IServiceManager _manager;

        public QuizController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index(QuizRequestParameters q)
        {
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

 

        public IActionResult Get([FromRoute(Name = "id")] int id)
        {
            var model = _manager.QuizService.GetOneQuiz(id, false);
            ViewData["Title"] = model?.Title;
            return View(model);
        }
    }
}
