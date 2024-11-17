using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizHubPresentation.Models;
using Services.Contracts;
using Services.Implementations;

namespace QuizHubPresentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IServiceManager _serviceManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IServiceManager serviceManager,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _serviceManager = serviceManager; 
            _emailSender = emailSender;
        }

        public IActionResult Login()
        {
            return View(new LoginModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");  

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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Kullanıcı bulunamazsa bile aynı mesajı vererek güvenliği artırırız.
                ViewBag.Message = "If the email exists in our system, a reset link has been sent.";
                return View("ForgotPasswordConfirmation");
            }

            // Şifre sıfırlama linki oluşturma
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            // E-posta gönderme işlemi
            await _emailSender.SendEmailAsync(model.Email, "Password Reset", $"Please reset your password by clicking here: {resetLink}");

            // Bilgilendirme mesajını göster
            ViewBag.Message = "If the email exists in our system, a reset link has been sent.";
            return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordDto { Token = token, Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Please correct the errors and try again.";

                return View(model);
            }

            var resetResult = await _serviceManager.AuthService.ResetPassword(model);

            if (!resetResult.IsSuccess)
            {
                TempData["danger"] = resetResult.Error; // Hata mesajı
                return View(model);
            }

            TempData["success"] = "Your password has been reset successfully!"; // Başarı mesajı
            return RedirectToAction("Login");
        }



    }
}
