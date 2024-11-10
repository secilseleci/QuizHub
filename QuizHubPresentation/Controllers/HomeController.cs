using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizHubPresentation.Infrastructure.Extensions;
using QuizHubPresentation.Models;
using Repositories.Contracts;
using Services.Contracts;
using System.Security.Claims;

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
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("UserDashboard"); // Giriş yapmış kullanıcılar için dashboard view
            }
            return View("GuestHome");
        }
   
         
        


    }
}