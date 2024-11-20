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
        public async Task<IActionResult> PendingQuizzes()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CompletedQuizzes()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RetakeQuizzes()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ContinueQuizzes()
        {
            return View();
        }

        [HttpGet]
       
        public async Task<IActionResult> StartQuizConfirmation(int quizId)
        {
            var quizResult = await _serviceManager.QuizService.GetQuizWithDetails(quizId, trackChanges: false);

            if (!quizResult.IsSuccess)
            {
                return NotFound("Quiz bulunamadı.");
            }

            ViewBag.QuizTitle = quizResult.Data.Title;
            ViewBag.QuizId = quizResult.Data.QuizId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ContinueQuiz(int quizId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quizResult = await _serviceManager.QuizService.ContinueQuiz(quizId, userId);

            if (!quizResult.IsSuccess)
            {
                return NotFound();
            }

            return View("QuizView", quizResult.Data);
        }
        
        [HttpPost]
        public async Task<IActionResult> StartQuiz(int quizId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quizResult = await _serviceManager.QuizService.StartQuiz(quizId, userId);

            if (!quizResult.IsSuccess)
            {
                return NotFound(quizResult.UserMessage);
            }

            return View("QuizView", quizResult.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAnswer(int quizId, int questionId, int selectedOptionId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _serviceManager.QuizService.SaveAnswer(quizId, questionId, selectedOptionId, userId);

            if (!result.IsSuccess)
            {
                return BadRequest(result.UserMessage);
            }

            return Json(new { success = true });
        }

        [HttpPost]
      
        public async Task<IActionResult> NextQuestion(int quizId, int currentQuestionOrder, int selectedOptionId)
        {
            var nextQuestionResult = await _serviceManager.QuestionService.GetNextQuestion(quizId, currentQuestionOrder, selectedOptionId);

            if (!nextQuestionResult.IsSuccess)
            {
                return Json(new
                {
                    success = false,
                    message = "Soru bulunamadı veya tüm sorular yanıtlandı."
                });
            }

            var questionToShow = nextQuestionResult.Data;

            var response = new
            {
                success = true,
                questionText = questionToShow.QuestionText,
                options = questionToShow.Options.Select(o => new
                {
                    id = o.OptionId,
                    text = o.OptionText,
                    isSelected = o.IsSelected, 
                    isDisabled = o.IsDisabled  
                }).ToList(),
                questionId = questionToShow.QuestionId,
                currentOrder = questionToShow.Order,
                totalQuestions = questionToShow.QuestionCount,
                showFinishButton = questionToShow.IsLastQuestion 
            };

            return Json(response);
        }


        [HttpPost]
  
        public async Task<IActionResult> FinishQuiz(int quizId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userQuizInfoResult = await _serviceManager.UserQuizInfoService.ProcessQuiz(quizId, userId);

            if (!userQuizInfoResult.IsSuccess)
            {
                return NotFound("Quiz işlenirken bir hata oluştu.");
            }

            var quizResultViewModel = _mapper.Map<QuizResultViewModel>(userQuizInfoResult.Data);
            TempData["QuizResult"] = JsonConvert.SerializeObject(quizResultViewModel);

            return RedirectToAction("QuizResult");
        }

        [HttpGet]
        public IActionResult QuizResult()
        {
            var quizResultJson = TempData["QuizResult"] as string;
        

            if (string.IsNullOrEmpty(quizResultJson))
            {
                return NotFound("Quiz sonucu bulunamadı.");
            }

            var quizResultViewModel = JsonConvert.DeserializeObject<QuizResultViewModel>(quizResultJson);
           
            return View(quizResultViewModel);
        }
    }
}