﻿using AutoMapper;
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

            var quizDto = new QuizDtoForUser
            {
                QuizId = quiz.QuizId,
                Title = quiz.Title,
                Questions = quiz.Questions,             // Sorular ve seçenekler
                QuestionCount = quiz.Questions.Count    // Toplam soru sayısı
            }; 
            
            return View(quizDto);
        }


        [HttpPost]
        [Authorize]
        public IActionResult NextQuestion(int quizId, int currentQuestionOrder)
        {
            Console.WriteLine($"Quiz ID: {quizId}, Current Question Order: {currentQuestionOrder}");

            // Quiz'i sorularıyla birlikte alıyoruz
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);

            if (quiz == null)
            {
                return NotFound();
            }

            // Şu anki sorunun Order'ına göre bir sonraki soruyu alıyoruz
            var nextQuestion = quiz.Questions.FirstOrDefault(q => q.Order == currentQuestionOrder + 1);

            // Eğer bir sonraki soru yoksa (quiz bittiğinde)
            if (nextQuestion == null)
            {
                return Json(new { success = false, message = "Quiz Completed" });  // Quiz tamamlandığında mesaj dönebiliriz
            }

            // Sıradaki soruyu JSON olarak döndürüyoruz
            return Json(new
            {
                success = true,
                questionText = nextQuestion.QuestionText,
                options = nextQuestion.Options.Select(o => o.OptionText).ToList(),
                currentOrder = nextQuestion.Order,  // Şu anki soru sırası (Order)
                totalQuestions = quiz.Questions.Count  // Toplam soru sayısı
            });
        }



    }
}