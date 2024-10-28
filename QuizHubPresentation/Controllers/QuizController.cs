using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizHubPresentation.Models;
using Repositories.Contracts;
using Services.Contracts;
using System.Security.Claims;

namespace QuizHubPresentation.Controllers
{
    public class QuizController:Controller
    {
        private readonly IServiceManager _serviceManager;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        public QuizController(IRepositoryManager manager, IMapper mapper, UserManager<ApplicationUser> userManager, IServiceManager serviceManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;
            _serviceManager = serviceManager;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PendingQuizzes()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CompletedQuizzes()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RetakeQuizzes()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public IActionResult ContinueQuizzes()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult StartQuizConfirmation(int quizId)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);           
          
            if (quiz == null)
            {
                return NotFound("Quiz bulunamadı.");
            }
            ViewBag.QuizTitle = quiz.Title;
            ViewBag.QuizId = quiz.QuizId;
            return View();
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult ContinueQuiz(int quizId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
 
            var quizDto = _serviceManager.QuizService.ContinueQuiz(quizId, userId);
            if (quizDto == null)
            {
                return NotFound();
            }

            return View("QuizView", quizDto);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult StartQuiz(int quizId)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);

            if (quiz == null)
            {
                return NotFound("Quiz bulunamadı.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userQuizInfoTemp = new UserQuizInfoTemp
            {
                UserId = userId,
                QuizId = quizId,
                IsCompleted = false,
                CorrectAnswer = 0,
                FalseAnswer = 0,
                StartedAt = DateTime.Now,
            };

            _serviceManager.UserQuizInfoTempService.CreateTempInfo(userQuizInfoTemp);

            var firstQuestion = quiz.Questions.OrderBy(q => q.Order).FirstOrDefault();
            if (firstQuestion == null)
            {
                return NotFound("Bu quiz için sorular bulunamadı.");
            }

             var quizDto = _mapper.Map<QuizDtoForUser>(quiz);
            quizDto.QuestionCount = quiz.Questions.Count;
            quizDto.Questions = new List<Question> { firstQuestion }; 

             return View("QuizView", quizDto);
        }


        [HttpPost]
        [Authorize]
        public IActionResult SaveAnswer(int quizId, int questionId, int selectedOptionId)
        {
            // Kullanıcı ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Kullanıcının geçici quiz bilgilerini alıyoruz
            var userQuizInfoTemp = _serviceManager.UserQuizInfoTempService.GetTempInfoByQuizIdAndUserId(quizId, userId, trackChanges: false);
            if (userQuizInfoTemp == null)
            {
                return BadRequest("Quiz bilgisi bulunamadı. Quiz'e başlamamış olabilirsiniz.");
            }

            // Soruyu alıyoruz ve doğruluk kontrolü yapıyoruz
            var question = _manager.Question.GetOneQuestion(questionId, trackChanges: false);
            if (question == null)
            {
                return BadRequest("Geçersiz soru ID'si.");
            }

            // Cevabın doğruluğunu kontrol et
            bool isCorrect = selectedOptionId == question.CorrectOptionId;

            // Geçici tabloya yeni cevabı kaydediyoruz
            var newAnswer = new UserAnswerTemp
            {
                UserQuizInfoTempId = userQuizInfoTemp.UserQuizInfoTempId,
                QuestionId = questionId,
                SelectedOptionId = selectedOptionId,
                IsCorrect = isCorrect
            };

            // Cevabı kaydediyoruz
            _serviceManager.UserAnswerTempService.CreateTempAnswer(newAnswer);

            return Json(new { success = true });
        }


        [HttpPost]
        [Authorize]
        public IActionResult NextQuestion(int quizId, int currentQuestionOrder)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);
            if (quiz == null)
            {
                return NotFound("Quiz bulunamadı.");
            }
            var nextQuestion = _serviceManager.QuizService.GetNextQuestion(quizId, currentQuestionOrder);
            if (nextQuestion == null)
            {
                return Json(new { success = false });
            }

            // Yeni soruyu JSON formatında frontend'e gönderiyoruz
            var response = new
            {
                success = true,
                questionText = nextQuestion.QuestionText,
                options = nextQuestion.Options.Select(o => new
                {
                    id = o.OptionId,
                    text = o.OptionText
                }).ToList(),
                questionId = nextQuestion.QuestionId,
                currentOrder = nextQuestion.Order,
                totalQuestions = quiz.Questions.Count
            };
             return Json(response);

        }


        [HttpPost]
        [Authorize]
        public IActionResult FinishQuiz(int quizId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. Serviste quiz'i işleyelim (hesapla, kaydet veya güncelle)
            var userQuizInfo = _serviceManager.UserQuizInfoService.ProcessQuiz(quizId, userId);
             // 3. QuizResult sayfasına yönlendirme
            var quizResultViewModel = _mapper.Map<QuizResultViewModel>(userQuizInfo);

            TempData["QuizResult"] = JsonConvert.SerializeObject(quizResultViewModel);
            return RedirectToAction("QuizResult");

        }
       

         
        [HttpGet]
        [Authorize]
        public IActionResult QuizResult()
        {
            // TempData'dan QuizResult verilerini alıyoruz
            var quizResultJson = TempData["QuizResult"] as string;

            // Eğer TempData'da veri yoksa, hata döndürüyoruz
            if (string.IsNullOrEmpty(quizResultJson))
            {
                return NotFound("Quiz sonucu bulunamadı.");
            }

            // JSON formatındaki veriyi deserialize ederek QuizResultViewModel'e dönüştürüyoruz
            var quizResultViewModel = JsonConvert.DeserializeObject<QuizResultViewModel>(quizResultJson);

            // Modeli View'e gönderiyoruz
            return View(quizResultViewModel);
        }
    }

    }
 
