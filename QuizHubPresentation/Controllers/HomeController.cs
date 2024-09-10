using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace QuizHubPresentation.Controllers
{

    public class HomeController : Controller
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
 
        public HomeController(IRepositoryManager manager, IMapper mapper )
        {
            _manager = manager;
            _mapper = mapper;
          
        }
        public IActionResult Index()
        {
            TempData["info"] = $"Welcome back, {DateTime.Now.ToShortTimeString()}";
            return View();
        }
        public IActionResult Details(int Id)
        {
            var quiz = _manager.Quiz.GetOneQuiz(Id, trackChanges: false);

            if (quiz == null)
            {
                return NotFound();   
            }

            var quizDto = _mapper.Map<QuizDtoForUserShowcase>(quiz);
            return View(quizDto);
        }

        [Authorize]  // Giriş yapmış kullanıcılar bu action'a erişebilir
        public IActionResult Start(int id)
        {
             var quiz = _manager.Quiz.GetOneQuiz(id, trackChanges: false);

            if (quiz == null)
            {
                return NotFound();
            }

            ViewBag.QuizTitle = quiz.Title;
            ViewBag.QuizId = quiz.QuizId;

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult StartQuiz(int quizId)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);
            if (quiz == null)
            {
                return NotFound();
            }

            var quizDto = _mapper.Map<QuizDtoForUser>(quiz);
            return View(quizDto);
        }


        [HttpPost]
        [Route("Home/NextQuestion")]
        public IActionResult NextQuestion(int quizId, int currentQuestionOrder)
        {

            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);

            if (quiz == null)
            {
                return NotFound();
            }
             var nextQuestion = quiz.Questions
                      .Where(q => q.Order > currentQuestionOrder)
                      .OrderBy(q => q.Order)
                      .FirstOrDefault();

            if (nextQuestion == null)
            {
                 return Json(new { success = false, message = "Quiz Completed" });  
            }

             return Json(new
            {
                success = true,
                questionText = nextQuestion.QuestionText,
                options = nextQuestion.Options.Select(o => o.OptionText).ToList(),
                currentOrder = nextQuestion.Order,  // Şu anki soru sırası (Order)
                totalQuestions = quiz.Questions.Count  // Toplam soru sayısı
            });
        }

        public IActionResult QuizCompleted()
        {
            return View();
        }


    }
}