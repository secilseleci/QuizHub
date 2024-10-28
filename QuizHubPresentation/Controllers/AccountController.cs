using DocumentFormat.OpenXml.ExtendedProperties;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizHubPresentation.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace QuizHubPresentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepositoryManager _manager;
        private readonly IServiceManager _serviceManager;
        public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,IRepositoryManager repositoryManager,IServiceManager serviceManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _manager = repositoryManager;
            _serviceManager = serviceManager;
        }

        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.Name);
                if (user is not null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                    {
                        return Redirect(model?.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("Error", "Invalid username or password.");
            }
            return View();
        }

        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(ReturnUrl);
        }

        public IActionResult Register()
        {
            var departments = _manager.Department.GetAllDepartments(false)
            .Select(d => new SelectListItem
            {
                Value = d.DepartmentId.ToString(),
                Text = d.DepartmentName
            }).ToList();


            ViewBag.Departments = departments;

            return View(new RegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                // Hataları görüntülemek için tekrar kayıt sayfasını döndür
                var departments = _serviceManager.DepartmentService.GetAllDepartments(false)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.DepartmentName
                    }).ToList();
                ViewBag.Departments = departments;
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DepartmentId = model.DepartmentId
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (roleResult.Succeeded)
                    return RedirectToAction("Login", new { ReturnUrl = "/" });
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            var departmentsList = _serviceManager.DepartmentService.GetAllDepartments(false)
       .Select(d => new SelectListItem
       {
           Value = d.DepartmentId.ToString(),
           Text = d.DepartmentName
       }).ToList();
            ViewBag.Departments = departmentsList;
            return View();
        }

       
    }
}