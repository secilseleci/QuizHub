using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace QuizHubPresentation.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepositoryManager _manager;

        private readonly IMapper _mapper;

        public HomeController(IRepositoryManager manager, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;
        }
            [AllowAnonymous]
            public IActionResult Index()
        {
            // Kullanıcı Admin rolündeyse Admin Index sayfasına yönlendir
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            // Kullanıcı giriş yapmışsa UserDashboard'u göster
            else if (User.Identity.IsAuthenticated)
            {
                return View("UserDashboard");
            }

            // Kullanıcı giriş yapmamışsa GuestHome'u göster
            return View("GuestHome");
        }
   
         
        


    }
}