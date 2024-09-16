using AutoMapper;
using Entities.Dtos;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models;
using Services.Contracts;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Entities.Models;


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

        [HttpGet]
        public IActionResult UploadExcel()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Excel dosyası yüklenmedi.");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);  
                    var rows = worksheet.RowsUsed();

                    var quizTitle = rows.Skip(1).First().Cell(1).Value.ToString();

                    var quiz = new Quiz
                    {
                        Title = quizTitle,
                        CreatedDate = DateTime.Now,
                        Questions = new List<Question>()
                    };

                    foreach (var row in rows.Skip(1))  
                    {
                        var questionText = row.Cell(2).Value.ToString();
                        var option1 = row.Cell(3).IsEmpty() ? null : row.Cell(3).Value.ToString(); 
                        var option2 = row.Cell(4).IsEmpty() ? null : row.Cell(4).Value.ToString();  
                        var option3 = row.Cell(5).IsEmpty() ? null : row.Cell(5).Value.ToString();  
                        var option4 = row.Cell(6).IsEmpty() ? null : row.Cell(6).Value.ToString();  
                        var option5 = row.Cell(7).IsEmpty() ? null : row.Cell(7).Value.ToString(); 

                        var correctAnswer = row.Cell(8).Value.ToString();  

                        var optionList = new List<Option>();

                        if (!string.IsNullOrWhiteSpace(option1))
                        {
                            optionList.Add(new Option { OptionText = option1, IsCorrect = (option1 == correctAnswer) });
                        }
                        if (!string.IsNullOrWhiteSpace(option2))
                        {
                            optionList.Add(new Option { OptionText = option2, IsCorrect = (option2 == correctAnswer) });
                        }
                        if (!string.IsNullOrWhiteSpace(option3))
                        {
                            optionList.Add(new Option { OptionText = option3, IsCorrect = (option3 == correctAnswer) });
                        }
                        if (!string.IsNullOrWhiteSpace(option4))
                        {
                            optionList.Add(new Option { OptionText = option4, IsCorrect = (option4 == correctAnswer) });
                        }
                        if (!string.IsNullOrWhiteSpace(option5))
                        {
                            optionList.Add(new Option { OptionText = option5, IsCorrect = (option5 == correctAnswer) });
                        }

                       
                        if (optionList.Count < 2)
                        {
                            continue; 
                        }

                        var question = new Question
                        {
                            QuestionText = questionText,
                            Quiz = quiz,
                            Options = optionList
                        };

                        var correctOption = optionList.FirstOrDefault(o => o.IsCorrect);
                        if (correctOption != null)
                        {
                            question.CorrectOptionId = correctOption.OptionId;  // Doğru cevabın OptionId'sini ayarla
                        }

                        quiz.Questions.Add(question);
                    }

                    var quizDto = _mapper.Map<QuizDtoForInsertion>(quiz);
                    _manager.QuizService.CreateQuiz(quizDto);
                }
            }
            return RedirectToAction("Index", "Quiz");
        }



    }
}