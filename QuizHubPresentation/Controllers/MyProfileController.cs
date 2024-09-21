using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizHubPresentation.Controllers
{
    public class MyProfileController : Controller
    {
        private readonly IUserQuizInfoService _userQuizInfoService; // Servis katmanı
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public MyProfileController(IUserQuizInfoService userQuizInfoService, IAuthService authService, IMapper mapper)
        {
            _userQuizInfoService = userQuizInfoService;
            _authService = authService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();

           
        }

    
 
        }
}
