using AutoMapper;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;

namespace QuizHubPresentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IServiceManager manager, IMapper mapper, ILogger<UserController> logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var users = await  _manager.AuthService.GetAllUsersWithRolesAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _manager.DepartmentService.GetAllDepartments(false);
            var roles = _manager.AuthService.Roles.Select(r => r.Name).ToList();

            // ViewBag ile Departments listesini view'e gönderiyoruz
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "Name");
            ViewBag.Roles = roles;

            return View(new UserDtoForCreation());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDtoForCreation userDto)
        {
            if (!ModelState.IsValid)
            {
                // Eğer validasyon hatası varsa, bilgileri tekrar yükleyip kullanıcıya geri döndür
                var departments = _manager.DepartmentService.GetAllDepartments(false);
                ViewBag.Departments = new SelectList(departments, "DepartmentId", "Name");

                var roles = _manager.AuthService.Roles.Select(r => r.Name).ToList();
                ViewBag.Roles = roles;

                return View(userDto);
            }

            // Eğer validasyon başarılıysa kullanıcıyı oluştur
            var result = await _manager.AuthService.CreateUser(userDto);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            // Hata olursa aynı şekilde bilgileri doldur
            var departmentsList = _manager.DepartmentService.GetAllDepartments(false);
            ViewBag.Departments = new SelectList(departmentsList, "DepartmentId", "Name");

            var rolesList = _manager.AuthService.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = rolesList;

            return View(userDto);
        }



        public async Task<IActionResult> Update([FromRoute(Name = "id")] string id)
        {
            var user = await _manager.AuthService.GetOneUserForUpdate(id);

            // Department listesi veritabanından çekiliyor
            var departments = _manager.DepartmentService.GetAllDepartments(false);

            // Seçilen departmentId'yi belirtiyoruz ve ViewModel'e ekliyoruz
            user.Departments = new SelectList(departments, "DepartmentId", "Name", user.DepartmentId);

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] UserDtoForUpdate userDto)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı bilgilerini güncelle
                await _manager.AuthService.UpdateUser(userDto);

                return RedirectToAction("Index");
            }

            return View(userDto);  // Hata durumunda tekrar formu göster
        }


        public async Task<IActionResult> ResetPassword([FromRoute(Name = "id")] string id)
        {
            return View(new ResetPasswordDto()
            {
                UserName = id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto model)
        {
            var result = await _manager.AuthService.ResetPassword(model);
            return result.Succeeded
                ? RedirectToAction("Index")
                : View();
        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOneUser([FromForm] UserDto userDto)
        {
            var result = await _manager
                .AuthService
                .DeleteOneUser(userDto.UserName);
            
            return result.Succeeded
                ? RedirectToAction("Index")
                : View();
        }
    }
}