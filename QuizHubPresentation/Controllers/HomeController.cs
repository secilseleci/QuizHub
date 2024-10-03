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
        private readonly IServiceManager _serviceManager;

        private readonly IMapper _mapper;
       
        public HomeController(IRepositoryManager manager, IMapper mapper,UserManager<ApplicationUser> userManager, IServiceManager serviceManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;
          _serviceManager = serviceManager;
        }
        public IActionResult Index()
        {
            return View();
        }
   
         
        


    }
}