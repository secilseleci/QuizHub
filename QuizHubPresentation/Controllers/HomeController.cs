using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Security.Claims;

namespace QuizHubPresentation.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
       
        public HomeController(IRepositoryManager manager, IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;
          
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            TempData["info"] = $"Welcome back, {DateTime.Now.ToShortTimeString()}";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.FindByIdAsync(userId).Result;
            var departmentId = user?.DepartmentId;

            if (departmentId == null)
            {
                return NotFound("Department not found for this user.");
            }
            var quizzes = _manager.Quiz.GetShowCaseQuizzes(false)
                           .Include(q => q.Departments)
                           .ToList();
            foreach (var quiz in quizzes)
            {
                Console.WriteLine($"Quiz Title: {quiz.Title}, QuizId: {quiz.QuizId}");
                foreach (var dept in quiz.Departments)
                {
                    Console.WriteLine($"DepartmentId: {dept.DepartmentId}");
                }
            }
            quizzes = quizzes.Where(q => q.Departments != null && q.Departments.Any(d => d.DepartmentId == departmentId)).ToList();
            foreach (var quiz in quizzes)
            {
                Console.WriteLine($"Filtered Quiz Title: {quiz.Title}, QuizId: {quiz.QuizId}");
            }

            return View(quizzes);
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

        
            bool isBlank = selectedOptionId == 0;  // Boş cevap kontrolü
            bool isCorrect = false;  // Varsayılan olarak cevap yanlış

            if (!isBlank)
            {
                var selectedOption = _manager.Option.GetOneOption(selectedOptionId, false);
                if (selectedOption == null)
                {
                    return BadRequest("Geçersiz seçenek ID'si.");
                }
                isCorrect = selectedOptionId == question.CorrectOptionId;  // Doğru cevap kontrolü
            }

            var userAnswer = new UserAnswer
            {
                UserQuizInfoId = userQuizInfoId.Value,
                QuestionId = questionId,
                SelectedOptionId = isBlank ? (int?)null : selectedOptionId,
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
         

        [HttpPost]
        [Authorize]
        public IActionResult FinishQuiz()
        {
            var userQuizInfoId = HttpContext.Session.GetInt32("UserQuizInfoId");
            if (!userQuizInfoId.HasValue)
            {
                return BadRequest("UserQuizInfoId bulunamadı. Quiz'e başlamamış olabilirsiniz.");
            }

            var userQuizInfo = _manager.UserQuizInfo.GetUserQuizInfoById(userQuizInfoId.Value,false);
            if (userQuizInfo == null)
            {
                return NotFound("UserQuizInfo bulunamadı.");
            }
            var quiz = GetQuizWithDetails(userQuizInfo.QuizId);   
            var totalQuestions = quiz.Questions.Count;

            var userAnswers = _manager.UserAnswer.GetUserAnswersByQuizInfoId(userQuizInfoId.Value, false);

            var correctAnswers = userAnswers.Count(a => a.IsCorrect);
            var falseAnswers = userAnswers.Count(a => !a.IsCorrect && a.SelectedOptionId != null);  
            var blankAnswers = userAnswers.Count(a => a.SelectedOptionId == null);  
            
            double successRate = ((double)correctAnswers / totalQuestions) * 100;
            bool isSuccessful = successRate >= 60;

            userQuizInfo.IsCompleted = true;  
            userQuizInfo.CompletedAt = DateTime.Now; 
            userQuizInfo.CorrectAnswer = correctAnswers;  
            userQuizInfo.FalseAnswer = falseAnswers;
            userQuizInfo.BlankAnswer = blankAnswers;
            userQuizInfo.IsSuccessful = isSuccessful;


            userQuizInfo.Score = (int)successRate;


            _manager.UserQuizInfo.Update(userQuizInfo);
            _manager.Save();

            return Json(new { success = true });
        }


        [Authorize]
        public IActionResult QuizCompleted()
        {
            // 1. UserQuizInfoId'yi session'dan al
            var userQuizInfoId = HttpContext.Session.GetInt32("UserQuizInfoId");
            if (!userQuizInfoId.HasValue)
            {
                return BadRequest("UserQuizInfoId bulunamadı.");
            }

            var userQuizInfo = _manager.UserQuizInfo.GetUserQuizInfoById(userQuizInfoId.Value, false);
            if (userQuizInfo == null)
            {
                return NotFound("UserQuizInfo bulunamadı.");
            }

            var quizInfoDto = _mapper.Map<UserQuizInfoDtoForCompleted>(userQuizInfo);
            return View(quizInfoDto);
        }

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