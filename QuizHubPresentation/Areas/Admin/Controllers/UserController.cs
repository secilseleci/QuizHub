using AutoMapper;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Services.Contracts;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IServiceManager serviceManager, IMapper mapper, ILogger<UserController> logger)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var usersResult = await _serviceManager.AuthService.GetAllUsersWithRolesAsync();

            if (!usersResult.IsSuccess)
            {
                _logger.LogError("Users could not be loaded: {Message}", usersResult.Error);
                TempData["ErrorMessage"] = "Users could not be loaded.";
                return View("Error");
            }

            return View(usersResult.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(trackChanges: false);
            if (!departmentsResult.IsSuccess || !departmentsResult.Data.Any())
            {
                ModelState.AddModelError("", "No departments found.");
                return View(new UserDtoForCreation());
            }

            ViewBag.Departments = new SelectList(departmentsResult.Data, "DepartmentId", "DepartmentName");

            var roles = _serviceManager.AuthService.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = roles;

            return View(new UserDtoForCreation());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDtoForCreation userDto)
        {
            if (!ModelState.IsValid)
            {
                var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(trackChanges: false);
                if (departmentsResult.IsSuccess)
                {
                    ViewBag.Departments = new SelectList(departmentsResult.Data, "DepartmentId", "DepartmentName");
                }

                ViewBag.Roles = _serviceManager.AuthService.Roles.Select(r => r.Name).ToList();
                return View(userDto);
            }

            var createResult = await _serviceManager.AuthService.CreateUser(userDto);

            if (createResult.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            var departments = await _serviceManager.DepartmentService.GetAllDepartments(trackChanges: false);
            if (departments.IsSuccess)
            {
                ViewBag.Departments = new SelectList(departments.Data, "DepartmentId", "DepartmentName");
            }

            ViewBag.Roles = _serviceManager.AuthService.Roles.Select(r => r.Name).ToList();
            ModelState.AddModelError("", "User could not be created.");
            return View(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var userResult = await _serviceManager.AuthService.GetOneUserForUpdate(id);
            if (!userResult.IsSuccess)
            {
                return NotFound();
            }

            var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(false);
            if (departmentsResult.IsSuccess)
            {
                ViewBag.Departments = new SelectList(departmentsResult.Data, "DepartmentId", "DepartmentName", userResult.Data.DepartmentId);
            }

            return View(userResult.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] UserDtoForUpdate userDto)
        {
            if (!ModelState.IsValid)
            {
                var departmentsResult = await _serviceManager.DepartmentService.GetAllDepartments(false);
                if (departmentsResult.IsSuccess)
                {
                    ViewBag.Departments = new SelectList(departmentsResult.Data, "DepartmentId", "DepartmentName", userDto.DepartmentId);
                }
                userDto.Roles = _serviceManager.AuthService.Roles.Select(r => r.Name).ToList();

                return View(userDto);
            }

            var updateResult = await _serviceManager.AuthService.UpdateUser(userDto);
            if (!updateResult.IsSuccess)
            {
                ModelState.AddModelError("", "User could not be updated.");
                return View(userDto);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AdminResetPassword(string username)
        {
            return View(new ResetPasswordDto { UserName = username });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Please correct the errors and try again.";

                return View(model);
            }
            var resetResult = await _serviceManager.AuthService.ResetPassword(model);

            if (!resetResult.IsSuccess)
            {
                TempData["danger"] = resetResult.Error; // Hata mesajý
                return View(model);
            }

            TempData["success"] = "User's password has been reset successfully!"; // Baþarý mesajý
            return RedirectToAction("Index", "User");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOneUser([FromForm] UserDto userDto)
        {
            var deleteResult = await _serviceManager.AuthService.DeleteOneUser(userDto.UserName);

            if (!deleteResult.IsSuccess)
            {
                ModelState.AddModelError("", "User deletion failed.");
                return View("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
