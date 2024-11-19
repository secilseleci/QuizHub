using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginModel());

        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)  
            {
                return View(model);  
            }
           
            var user = await _userManager.FindByNameAsync(model.Username);
            
            if (user == null)
            {
                TempData["danger"] = "Invalid username or password.";  
                return View(model);
             }
             
            await _signInManager.SignOutAsync();
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
           
            if (!signInResult.Succeeded)
            {
                TempData["danger"] = "Invalid username or password.";  
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsIntoViewBag();
                TempData["danger"] = "Please correct the errors and try again."; 
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
                {
                    TempData["success"] = "Registration successful! You can now log in.";  
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["danger"] = "Failed to assign role to the user.";  
                }
            }
            TempData["danger"] = "Registration failed. Please try again.";  

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
            else
            {
                TempData["danger"] = "Failed to load departments. Please try again later.";
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Please check the form and try again.";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Password Reset", $"Please reset your password by clicking here: {resetLink}");

            TempData["info"] = "If the email exists in our system, a reset link has been sent.";
            return View("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Error", "Home", new { statusCode = 400 });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 404 });
            }

            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, "Default", "ResetPassword", token);
            if (!isTokenValid)
            {
                return RedirectToAction("Error", "Home", new { statusCode = 403 });
            }

            return View(new ResetPasswordDto { Token = token, Email = email });
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Please correct the errors and try again.";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["danger"] = "Invalid request.";
                return RedirectToAction("ForgotPassword");
            }

            var resetResult = await _serviceManager.AuthService.ResetPassword(model);

            if (!resetResult.IsSuccess)
            {
                TempData["danger"] = resetResult.Error;  
                return View(model);
            }

            TempData["success"] = "Your password has been reset successfully!";  
            return RedirectToAction("Login");
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User); 
            var profileResult = await _serviceManager.UserProfileService.GetProfileAsync(userId);

            if (!profileResult.IsSuccess)
            {
                ModelState.AddModelError("", profileResult.UserMessage);
                return RedirectToAction("Error", "Home");
            }

            return View(profileResult.Data);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileUpdateDto model)
        {
            var userId = _userManager.GetUserId(User);

            var updateResult = await _serviceManager.UserProfileService.UpdateProfileAsync(model, userId);

            if (!updateResult.IsSuccess)
            {
                TempData["danger"] = updateResult.UserMessage;
                return RedirectToAction("Profile");
            }

            var user = await _userManager.FindByIdAsync(userId);
            await _signInManager.RefreshSignInAsync(user);

            TempData["success"] = "Profile updated successfully.";
            return RedirectToAction("Profile");
        }




        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Please fix the errors and try again.";
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var updateResult = await _serviceManager.UserProfileService.UpdatePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

            if (updateResult.IsSuccess)
            {
                TempData["success"] = updateResult.UserMessage;
                return RedirectToAction("Profile");
            }
            else
            {
                TempData["danger"] = updateResult.UserMessage;
                return View(model);
            }
        }



    }
}
