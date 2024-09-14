using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using System.Security.Claims;

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

        public IActionResult Details(int id)
        {
            var quiz = GetQuizWithDetails(id);
            var quizDto = _mapper.Map<QuizDtoForUserShowcase>(quiz);
            return View(quizDto);
        }


        [Authorize]   
        public IActionResult Start(int id)
        {
            var quiz = GetQuizWithDetails(id);
            ViewBag.QuizTitle = quiz.Title;
            ViewBag.QuizId = quiz.QuizId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult StartQuiz(int quizId)
        {
            var quiz = GetQuizWithDetails(quizId);
            var quizDto = _mapper.Map<QuizDtoForUser>(quiz);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  
           
            var userQuizInfo = new UserQuizInfo
            {
                UserId = userId,
                QuizId = quizId,
                IsCompleted = false,
                CorrectAnswer = 0,
                FalseAnswer = 0,
                BlankAnswer = 0,
                Score = 0
            };
            _manager.UserQuizInfo.CreateOneUserQuizInfo(userQuizInfo);
            _manager.Save();  
            HttpContext.Session.SetInt32("UserQuizInfoId", userQuizInfo.UserQuizInfoId);

            return View(quizDto);
        }


        [HttpPost]
        [Authorize]
        public IActionResult SaveAnswer(int quizId, int questionId, int selectedOptionId)
        {
            var userQuizInfoId = HttpContext.Session.GetInt32("UserQuizInfoId");
            if (!userQuizInfoId.HasValue)
            {
                return BadRequest("UserQuizInfoId bulunamadı. Quiz'e başlamamış olabilirsiniz.");
            }

            var question = _manager.Question.GetOneQuestion(questionId, false);
            if (question == null)
            {
                return BadRequest("Geçersiz soru ID'si.");
            }

            var selectedOption = _manager.Option.GetOneOption(selectedOptionId, false);
            if (selectedOption == null)
            {
                return BadRequest("Geçersiz seçenek ID'si.");
            }

            var isCorrect = selectedOptionId == question.CorrectOptionId;  // Doğru cevap kontrolü

            var userAnswer = new UserAnswer
            {
                UserQuizInfoId = userQuizInfoId.Value,
                QuestionId = questionId,
                SelectedOptionId = selectedOptionId,
                IsCorrect = isCorrect  // Doğru olup olmadığını burada belirliyoruz
            };

            _manager.UserAnswer.CreateUserAnswer(userAnswer);
            _manager.Save();  // Veritabanına kaydediyoruz

            return Json(new { success = true });
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
                options = nextQuestion.Options.Select(o => new { id = o.OptionId, text = o.OptionText }).ToList(),  // OptionId ve OptionText
                questionId = nextQuestion.QuestionId,
                currentOrder = nextQuestion.Order,  // Şu anki soru sırası (Order)
                totalQuestions = quiz.Questions.Count  // Toplam soru sayısı
            });

        }
        public IActionResult QuizCompleted()
        {
            return View();
        }


        // Private metot: Sık tekrarlanan quiz sorgusu için
        private Quiz GetQuizWithDetails(int quizId)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);
            if (quiz == null)
            {
                throw new Exception("Quiz bulunamadı.");
            }
            return quiz;
        }
    }
}