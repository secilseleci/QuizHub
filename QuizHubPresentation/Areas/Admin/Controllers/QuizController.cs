using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models;
using Services;
using Services.Contracts;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class QuizController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly IMapper _mapper;
        public QuizController(IServiceManager manager,IMapper mapper)

        {
            _manager = manager; 
            _mapper = mapper;
        }

        //[HttpPost("add-quiz-from-json")]
        //public IActionResult AddQuizFromJson([FromBody] string jsonData)
        //{
        //    _manager.QuizService.AddQuizFromJson(jsonData);  // Servisteki metodu çağırıyoruz
        //    return Ok(new { message = "Quiz başarıyla eklendi!" });
        //}

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
            var quizDto = new QuizDtoForInsertion
            {
                Questions = new List<QuestionDtoForInsertion>
                    {
                        new QuestionDtoForInsertion
                        {
                            Order = 1,
                            Options = new List<OptionDtoForInsertion>
                                {
                                new OptionDtoForInsertion { OptionText = string.Empty, IsCorrect = false },
                                new OptionDtoForInsertion { OptionText = string.Empty, IsCorrect = false },
                                new OptionDtoForInsertion { OptionText = string.Empty, IsCorrect = false },
                                new OptionDtoForInsertion { OptionText = string.Empty, IsCorrect = false }
                }
            }
        }
            };

            TempData["info"] = "Please fill the form.";
            return View(quizDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] QuizDtoForInsertion quizDto)
        {
            if (quizDto.Questions == null || quizDto.Questions.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "En az bir soru eklemeniz gerekiyor.");
                return View(quizDto);
            }

            bool correctOptionSelected = true;

            foreach (var question in quizDto.Questions)
            {
                if (question.CorrectOptionId < 0 || question.CorrectOptionId >= question.Options.Count)
                {
                    correctOptionSelected = false;
                    ModelState.AddModelError(string.Empty, $"Soru '{question.QuestionText}' için bir doðru seçenek seçmelisiniz.");
                    return View(quizDto);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(quizDto);
            }

            foreach (var question in quizDto.Questions)
            {
                if (question.CorrectOptionId >= 0 && question.CorrectOptionId < question.Options.Count)
                {
                    question.Options[question.CorrectOptionId].IsCorrect = true;
                }
            }

            _manager.QuizService.CreateQuiz(quizDto);
            TempData["success"] = $"{quizDto.Title} baþarýyla oluþturuldu.";
            return RedirectToAction("Index");
        }

        




        public IActionResult Update([FromRoute(Name = "id")] int id)
        {

            var model = _manager.QuizService.GetOneQuizForUpdate(id, false);
            if (model == null)
            {
                return NotFound();
            }
            var quizDto = _mapper.Map<QuizDtoForUpdate>(model);
            ViewData["Title"] = model?.Title;
            return View(quizDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] QuizDtoForUpdate quizDto)
        {
            if (ModelState.IsValid)
            {
                var existingQuiz = _manager.QuizService.GetOneQuizForUpdate(quizDto.QuizId, false);
                quizDto.QuestionCount = existingQuiz.QuestionCount;

                foreach (var question in quizDto.Questions)
                {
                    var existingQuestion = existingQuiz.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
                    if (existingQuestion != null)
                    {
                        question.Order = existingQuestion.Order;

                        foreach (var option in question.Options)
                        {
                            var existingOption = existingQuestion.Options.FirstOrDefault(o => o.OptionId == option.OptionId);
                            if (existingOption != null)
                            {
                                option.IsCorrect = false;
                            }
                        }
                        var correctOption = question.Options.FirstOrDefault(o => o.OptionId == question.CorrectOptionId);
                        if (correctOption != null)
                        {
                            correctOption.IsCorrect = true;
                        }
                    }
                }

                _manager.QuizService.UpdateOneQuiz(quizDto);

                TempData["success"] = "Quiz baþarýyla güncellendi.";
                return RedirectToAction("Index");
            }

            return View(quizDto);
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