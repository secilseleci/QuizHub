using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Models;
using Services.Contracts;


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



        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Quizzes";

            var quizzesResult = await _manager.QuizService.GetAllQuizzes(false);
            if (!quizzesResult.IsSuccess)
            {
                TempData["error"] = quizzesResult.UserMessage;
                return View(new QuizListViewModel());
            }

            var model = new QuizListViewModel
            {
                Quizzes = quizzesResult.Data
            };

            return View(model);
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

            TempData["info"] = "Lütfen formu doldurun.";
            return View(quizDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] QuizDtoForInsertion quizDto)
        {
            if (!ModelState.IsValid)
            {
                return View(quizDto);
            }

            var result = await _manager.QuizService.CreateOneQuiz(quizDto);

            if (result.IsSuccess)
            {
                TempData["success"] = $"{quizDto.Title} başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, result.UserMessage);
            return View(quizDto);
        }



        [HttpGet]
        public async Task<IActionResult> Update([FromRoute(Name = "id")] int id)
        {
            var result =await _manager.QuizService.GetOneQuizForUpdate(id, trackChanges: false);
            if (!result.IsSuccess)
            {
                TempData["error"] = "Güncellemek istediğiniz quiz bulunamadı.";
                return RedirectToAction("Index");
            }

            var quizDto = result.Data;
            ViewData["Title"] = quizDto.Title;
            return View(quizDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] QuizDtoForUpdate quizDto)
        {
            if (!ModelState.IsValid)
            {
                return View(quizDto);
            }

            var result = await _manager.QuizService.UpdateOneQuiz(quizDto);

            if (result.IsSuccess)
            {
                TempData["success"] = "Quiz başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            // Hata durumunda kullanıcıya mesaj gösteriyoruz
            ModelState.AddModelError(string.Empty, result.UserMessage);
            return View(quizDto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] int id)
        {
            var result = await _manager.QuizService.DeleteOneQuiz(id);

            if (result.IsSuccess)
            {
                TempData["danger"] = "The quiz has been removed.";
            }
            else
            {
                TempData["error"] = result.UserMessage ?? "An error occurred while deleting the quiz.";
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult UploadExcel()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _manager.QuizService.UploadExcel(file);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.UserMessage;
                return RedirectToAction("UploadExcel");
            }

            TempData["success"] = "Quiz başarıyla yüklendi!";
            return RedirectToAction("Index", "Quiz");
        }

        [HttpGet]
        public async Task<IActionResult> Assign(int id)
        {
            if (id == 0)
            {
                TempData["error"] = "Quiz bulunamadı.";
                return RedirectToAction("Index");
            }

            // Quiz'i getiriyoruz
            var quizResult = await _manager.QuizService.GetOneQuiz(id, trackChanges: false);
            if (!quizResult.IsSuccess)
            {
                TempData["error"] = quizResult.UserMessage;
                return RedirectToAction("Index");
            }

            // Tüm departmanları getiriyoruz ve seçili olanları işaretliyoruz
            var departmentsResult = await _manager.DepartmentService.GetAllDepartmentsWithSelection(id, trackChanges: false);
            if (!departmentsResult.IsSuccess)
            {
                TempData["error"] = departmentsResult.UserMessage;
                return RedirectToAction("Index");
            }

            var model = new AssignQuizViewModel
            {
                QuizId = id,
                QuizTitle = quizResult.Data.Title,
                Departments = departmentsResult.Data // SelectListItem listesi
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Assign(AssignQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Formda geçersiz veriler var.";
                return View(model);
            }

            // Seçili departman ID'lerini topluyoruz
            var selectedDepartmentIds = model.SelectedDepartments.Select(int.Parse).ToList();
            var result = await _manager.QuizService.AssignQuizToDepartments(model.QuizId, selectedDepartmentIds);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.UserMessage;
                return View(model);
            }

            TempData["success"] = "Quiz başarıyla departmanlara atandı.";
            return RedirectToAction("Index");
        }

    }
}