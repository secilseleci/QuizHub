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
        UserManager<ApplicationUser> userManager){
            
        _manager = manager;
        _mapper = mapper;
        _userManager = userManager;}
            

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            else if (User.Identity.IsAuthenticated)
            {
                return View("UserDashboard");
            }

            return View("GuestHome");
        }


        [HttpGet("api/weather")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWeather()
        {
            string cityName = "Istanbul"; // Varsayılan şehir
            string apiKey = "adab173a0f78ebe3515f89888f4dad8c"; 

            // OpenWeatherMap API endpointi
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return Ok(json); // Hava durumu bilgisi JSON olarak döndürülür
                    }

                    return BadRequest("Unable to fetch weather data");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }



    }
}