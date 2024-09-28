using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Services.Contracts;

namespace QuizHubPresentation.Controllers
{
    public class QuizController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        public QuizController(IRepositoryManager manager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;

        }
        public IActionResult PendingQuizzes()
        {
            return View(); // pendingQuizzes view'ına yönlendirilir
        }

        public IActionResult CompletedQuizzes()
        {
            return View(); // pendingQuizzes view'ına yönlendirilir
        }

        public IActionResult RetakeQuizzes()
        {
            return View(); // pendingQuizzes view'ına yönlendirilir
        }
    }
}
