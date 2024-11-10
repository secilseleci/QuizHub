using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizHubPresentation.Models;
using Services.Contracts;

namespace QuizHubPresentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IServiceManager _serviceManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IServiceManager serviceManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceManager = serviceManager;
        }

        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string returnUrl = "/")
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return Redirect(model?.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("Error", "Invalid username or password.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Register()
        {
            var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(trackChanges: false);

            if (!departmentsResult.IsSuccess || departmentsResult.Data == null)
            {
                ModelState.AddModelError("", "No departments found.");
                return View(new RegisterDto());
            }

            ViewBag.Departments = new SelectList(departmentsResult.Data, "DepartmentId", "DepartmentName");
            return View(new RegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsIntoViewBag();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DepartmentId = model.DepartmentId
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (createResult.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                    return RedirectToAction("Login", new { ReturnUrl = "/" });
            }

            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            await LoadDepartmentsIntoViewBag();
            return View(model);
        }

        private async Task LoadDepartmentsIntoViewBag()
        {
            var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(trackChanges: false);
            if (departmentsResult.IsSuccess && departmentsResult.Data != null)
            {
                ViewBag.Departments = new SelectList(departmentsResult.Data, "DepartmentId", "DepartmentName");
            }
        }
    }
}
