using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Infrastructure.Extensions;
using QuizHubPresentation.Models;
using Repositories.Contracts;
using Services;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return Content("User not found");

            var departmentId = user.DepartmentId;

            // Departmana atanmış quizleri getiriyoruz
            var quizzes = _serviceManager.QuizService
                .GetQuizzesWithDepartments(trackChanges: false)
                .Where(q => q.ShowCase && q.Departments.Any(d => d.DepartmentId == departmentId))
                .ToList();

            // Kullanıcının daha önce çözmediği quizleri bulmak için `UserQuizInfo` kayıtlarına bakıyoruz
            var solvedQuizIds = _serviceManager.UserQuizInfoService.GetUserQuizInfoByUserId(userId, trackChanges: false)
                .Select(uqi => uqi.QuizId)
                .ToList();

            var pendingQuizzes = quizzes.Where(q => !solvedQuizIds.Contains(q.QuizId)).ToList();

            return View("PendingQuizzes", pendingQuizzes);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CompletedQuizzes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var completedQuizzes = _serviceManager.UserQuizInfoService
                .GetUserQuizInfoByUserId(userId, trackChanges: false)
                .Where(uqi => uqi.IsCompleted && (uqi.Score >= 60 || uqi.IsSuccessful))
                .ToList();

            return View("CompletedQuizzes", completedQuizzes);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RetakeQuizzes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var retakeQuizzes = _serviceManager.UserQuizInfoService
                .GetUserQuizInfoByUserId(userId, trackChanges: false)
                .Where(uqi => uqi.IsCompleted && (uqi.Score < 60 || (uqi.Score < 100 && uqi.IsSuccessful)))
                .ToList();

            return View("RetakeQuizzes", retakeQuizzes);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ContinueQuizzes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var continueQuizzes = _serviceManager.UserQuizInfoService
                .GetUserQuizInfoByUserId(userId, trackChanges: false)
                .Where(uqi => !uqi.IsCompleted)
                .ToList();

            return View("ContinueQuizzes", continueQuizzes);
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

        [HttpPost]
        [Authorize]
        public IActionResult StartQuiz(int quizId)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);

            if (quiz == null)
            {
                return NotFound("Quiz bulunamadı.");
            }
            var quizDto = _mapper.Map<QuizDtoForUser>(quiz);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userQuizInfo = new UserQuizInfo
            {
                UserId = userId,
                QuizId = quizId,
                IsCompleted = false,
                CorrectAnswer = 0,
                FalseAnswer = 0,
                Score = 0
            };

            HttpContext.Session.SetJson("UserQuizInfo", userQuizInfo);
            HttpContext.Session.SetInt32("CurrentQuestionOrder", 1);
            HttpContext.Session.SetJson("TemporaryAnswers", new List<UserAnswerTemp>());

            // İlk soruyu döndürüyoruz
            var firstQuestion = quiz.Questions.OrderBy(q => q.Order).FirstOrDefault();
            if (firstQuestion == null)
            {
                return NotFound("Bu quiz için sorular bulunamadı.");
            }

            // İlk soruyu ve seçenekleri model olarak view'e gönderiyoruz
            quizDto.QuestionCount = quiz.Questions.Count;
            quizDto.Questions = new List<Question> { firstQuestion };  // Sadece ilk soruyu gönderiyoruz

            return View("QuizView", quizDto);
        }


        [HttpPost]
        [Authorize]
        public IActionResult SaveAnswer(int quizId, int questionId, int selectedOptionId)
        {
            // 1. Kullanıcının session'daki bilgilerini alıyoruz
            var userQuizInfo = HttpContext.Session.GetJson<UserQuizInfo>("UserQuizInfo");
            if (userQuizInfo == null)
            {
                return BadRequest("Quiz bilgisi bulunamadı. Quiz'e başlamamış olabilirsiniz.");
            }

            // 2. Kullanıcının geçici cevaplarını içeren session'daki listeyi alıyoruz
            var tempAnswers = HttpContext.Session.GetJson<List<UserAnswerTemp>>("TemporaryAnswers") ?? new List<UserAnswerTemp>();

            // 3. Şu anki sorunun doğru cevabını bulmak için veritabanından question ve correctOptionId'yi alıyoruz
            var question = _manager.Question.GetOneQuestion(questionId, trackChanges: false);
            if (question == null)
            {
                return BadRequest("Geçersiz soru ID'si.");
            }

            // 4. Cevap doğruluğunu kontrol et
            bool isCorrect = false;
 
             isCorrect = selectedOptionId == question.CorrectOptionId;
          

            // 5. Geçici cevabı session'daki listede güncelliyoruz
            var existingAnswer = tempAnswers.FirstOrDefault(a => a.QuestionId == questionId);
            if (existingAnswer != null)
            {
                // Eğer cevap zaten varsa güncelliyoruz
                existingAnswer.SelectedOptionId = selectedOptionId;
                existingAnswer.IsCorrect = isCorrect;
            }
            else
            {
                // Eğer cevap yoksa yeni bir cevap ekliyoruz
                tempAnswers.Add(new UserAnswerTemp
                {
                    QuestionId = questionId,
                    SelectedOptionId = selectedOptionId,
                    IsCorrect = isCorrect
                });
            }

            // 6. Güncellenen cevapları session'a kaydediyoruz
            HttpContext.Session.SetJson("TemporaryAnswers", tempAnswers);

            // 7. Kullanıcının bir sonraki soruya geçebilmesi için current question order'ı güncelliyoruz
            var currentQuestionOrder = HttpContext.Session.GetInt32("CurrentQuestionOrder") ?? 1;
            HttpContext.Session.SetInt32("CurrentQuestionOrder", currentQuestionOrder + 1);

            return Json(new { success = true, message = "Cevabınız kaydedildi." });
        }



        [HttpPost]
        [Authorize]
        public IActionResult NextQuestion(int quizId, int currentQuestionOrder)
        {
            // 1. Session'daki bilgileri alıyoruz
            var userQuizInfo = HttpContext.Session.GetJson<UserQuizInfo>("UserQuizInfo");
            var tempAnswers = HttpContext.Session.GetJson<List<UserAnswerTemp>>("TemporaryAnswers");

            if (userQuizInfo == null || tempAnswers == null)
            {
                return BadRequest("Session'da quiz bilgisi bulunamadı. Quiz'e başlamamış olabilirsiniz.");
            }

            // 2. Veritabanından quiz detaylarını alıyoruz
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);
            if (quiz == null)
            {
                return NotFound("Quiz bulunamadı.");
            }

            // 3. Sıradaki soruyu buluyoruz
            var nextQuestion = quiz.Questions
                .Where(q => q.Order > currentQuestionOrder) // Şu anki sıradan büyük olan soruları buluyoruz
                .OrderBy(q => q.Order)  // Sıradaki en küçük order'ı bul
                .FirstOrDefault();

            if (nextQuestion == null)
            {
                // Daha fazla soru kalmadıysa success = false döndür ve finish butonunu göster
                return Json(new { success = false });
            }

            // 4. Session'da current question order'ı güncelliyoruz
            HttpContext.Session.SetInt32("CurrentQuestionOrder", nextQuestion.Order);

            // 5. Sıradaki sorunun bilgilerini JSON formatında döndürüyoruz
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
        public IActionResult FinishQuiz()
        {
            var sessionQuizInfo = HttpContext.Session.GetJson<UserQuizInfo>("UserQuizInfo");
            var tempAnswers = HttpContext.Session.GetJson<List<UserAnswerTemp>>("TemporaryAnswers");

            if (sessionQuizInfo == null || tempAnswers == null || tempAnswers.Count == 0)
            {
                return BadRequest("Quiz bilgileri eksik veya cevaplar bulunamadı.");
            }

            // 2. Session'daki verilerle Score, CorrectAnswer, FalseAnswer, BlankAnswer ve IsSuccessful bilgisi hesaplanıyor
            sessionQuizInfo.CorrectAnswer = tempAnswers.Count(a => a.IsCorrect);
            sessionQuizInfo.FalseAnswer = tempAnswers.Count(a => !a.IsCorrect && a.SelectedOptionId != 0);

            int totalQuestions = sessionQuizInfo.CorrectAnswer + sessionQuizInfo.FalseAnswer;
            sessionQuizInfo.Score = (int)Math.Floor((double)sessionQuizInfo.CorrectAnswer / totalQuestions * 100);
            sessionQuizInfo.IsSuccessful = sessionQuizInfo.Score >= 60;
            sessionQuizInfo.CompletedAt = DateTime.Now;
            sessionQuizInfo.IsCompleted = true;
            HttpContext.Session.SetJson("UserQuizInfo", sessionQuizInfo);

            // Veritabanında bu quiz'e ve bu user'a ait önceki bir kayıt var mı diye kontrol edilir
            var userQuizInfoInDb = _serviceManager.UserQuizInfoService
                .GetUserQuizInfoByQuizIdAndUserId(sessionQuizInfo.QuizId, sessionQuizInfo.UserId, trackChanges: false);

            // Eğer daha önce kayıt yoksa, yeni bir kayıt oluşturulur
            if (userQuizInfoInDb == null)
            {
                // Yeni kayıt oluşturuluyor
                _serviceManager.UserQuizInfoService.CreateOneUserQuizInfo(sessionQuizInfo);

                // Cevaplar kaydediliyor
                foreach (var answer in tempAnswers)
                {
                    var userAnswer = new UserAnswer
                    {
                        UserQuizInfoId = sessionQuizInfo.UserQuizInfoId,
                        QuestionId = answer.QuestionId,
                        SelectedOptionId = answer.SelectedOptionId,
                        IsCorrect = answer.IsCorrect
                    };
                    _serviceManager.UserAnswerService.CreateUserAnswer(userAnswer);
                }

                // Sonuç sayfasına yönlendiriliyor
                return RedirectToAction("QuizResult");
            }

            //  Eğer daha önce kayıt varsa score karşılaştırması yapılır
            if (userQuizInfoInDb.Score < sessionQuizInfo.Score)
            {
                // Databasedeki skor düşükse: hem UserQuizInfo hem de UserAnswers güncellenir
                userQuizInfoInDb.Score = sessionQuizInfo.Score;
                userQuizInfoInDb.IsSuccessful = sessionQuizInfo.IsSuccessful;
                userQuizInfoInDb.CompletedAt = DateTime.Now;
                userQuizInfoInDb.CorrectAnswer = sessionQuizInfo.CorrectAnswer;

                userQuizInfoInDb.FalseAnswer = sessionQuizInfo.FalseAnswer;

                _serviceManager.UserQuizInfoService.UpdateOneUserQuizInfo(userQuizInfoInDb);

                foreach (var answer in tempAnswers)
                {
                    var existingAnswer = _serviceManager.UserAnswerService
                        .GetUserAnswer(userQuizInfoInDb.UserQuizInfoId, answer.QuestionId, trackChanges: true);

                    if (existingAnswer != null)
                    {
                        // Mevcut cevap güncellenir
                        existingAnswer.SelectedOptionId = answer.SelectedOptionId;
                        existingAnswer.IsCorrect = answer.IsCorrect;
                        _serviceManager.UserAnswerService.UpdateUserAnswer(existingAnswer);
                    }
                    else
                    {
                        // Yeni cevap eklenir
                        var userAnswer = new UserAnswer
                        {
                            UserQuizInfoId = userQuizInfoInDb.UserQuizInfoId,
                            QuestionId = answer.QuestionId,
                            SelectedOptionId = answer.SelectedOptionId,
                            IsCorrect = answer.IsCorrect
                        };
                        _serviceManager.UserAnswerService.CreateUserAnswer(userAnswer);
                    }
                }

                return RedirectToAction("QuizResult");
            }
            else if (userQuizInfoInDb.Score == sessionQuizInfo.Score)
            {
                // Databasedeki skor eşitse: sadece CompletedAt güncellenir
                userQuizInfoInDb.CompletedAt = sessionQuizInfo.CompletedAt;
                _serviceManager.UserQuizInfoService.UpdateOneUserQuizInfo(userQuizInfoInDb);
                return RedirectToAction("QuizResult");

            }

            // Databasedeki skor daha yüksekse: hiçbir işlem yapılmaz, direkt sonuç sayfasına yönlendirilir
            return RedirectToAction("QuizResult");
        }

        [HttpGet]
        [Authorize]
        public IActionResult QuizResult()
        {
            // Session'daki Quiz sonucu direkt olarak gösterilecek
            var sessionQuizInfo = HttpContext.Session.GetJson<UserQuizInfo>("UserQuizInfo");

            if (sessionQuizInfo == null)
            {
                return NotFound("Sonuç bilgisi bulunamadı.");
            }

            return View(sessionQuizInfo);  // Session'daki verilerle sonuç gösterilecek

        }
    }
}
