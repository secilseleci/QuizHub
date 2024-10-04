using AutoMapper;
using ClosedXML.Excel;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizHubPresentation.Models;
using Services.Contracts;
using Microsoft.AspNetCore.Identity;


namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class QuizController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceManager _manager;
        private readonly IMapper _mapper;

        public QuizController(IServiceManager manager,IMapper mapper, UserManager<ApplicationUser> userManager)
            
        {
            _manager = manager; 
            _mapper = mapper;
            _userManager = userManager;
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
        
        [HttpGet]
        public IActionResult Create()
        {
            var quizDto = new QuizDtoForInsertion
            {
                Questions = new List<QuestionDto>
                    {
                        new QuestionDto
                        {
                            Order = 1,
                            Options = new List<OptionDto>
                                {
                                new OptionDto { OptionText = string.Empty, IsCorrect = false },
                                new OptionDto { OptionText = string.Empty, IsCorrect = false },
                                new OptionDto { OptionText = string.Empty, IsCorrect = false },
                                new OptionDto { OptionText = string.Empty, IsCorrect = false }
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
                var optionList = question.Options.ToList();

                if (question.CorrectOptionId < 0 || question.CorrectOptionId >= optionList.Count)
                {
                    correctOptionSelected = false;
                    ModelState.AddModelError(string.Empty, $"Soru '{question.QuestionText}' için bir doğru seçenek seçmelisiniz.");
                    return View(quizDto);
                }

                if (question.CorrectOptionId >= 0 && question.CorrectOptionId < optionList.Count)
                {
                    optionList[question.CorrectOptionId].IsCorrect = true;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(quizDto);
            }

            _manager.QuizService.CreateQuiz(quizDto);
            TempData["success"] = $"{quizDto.Title} başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Assign(int id)
        {
            if (id == 0)
            {
                throw new Exception("Geçersiz quizId değeri!");
            }

            var quiz = _manager.QuizService.GetOneQuiz(id, false);
            if (quiz == null)
            {
                throw new Exception("Quiz bulunamadı!");
            }

            //Quiz'e atanmış olan departmanları alıyoruz
            var assignedDepartments = _manager.QuizService.GetDepartmentsByQuizId(id, false);
            var allDepartments = _manager.DepartmentService.GetAllDepartments(false)
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.DepartmentName,
                    Selected = assignedDepartments.Any(ad => ad.DepartmentId == d.DepartmentId) // Atanmışsa işaretle
                }).ToList();

            if (allDepartments == null || !allDepartments.Any())
            {
                throw new Exception("Departmanlar listesi boş!");
            }

            var model = new AssignQuizViewModel
            {
                QuizId = id,
                QuizTitle = quiz.Title,
                Departments = allDepartments // Departmanlar listesini view'e gönder
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(AssignQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Formdan gelen seçili departmanlar (checkbox'ta işaretlenenler)
            var selectedDepartmentIds = model.SelectedDepartments.Select(int.Parse).ToList();

            // 2. Seçilen departmanlar quiz'e atanıyor, var olan ilişkiler güncelleniyor
            _manager.QuizService.AssignQuizToDepartments(model.QuizId, selectedDepartmentIds);

            // 3. İşlem tamamlandığında Index sayfasına yönlendiriyoruz
            return RedirectToAction("Index");
        }




    }
}