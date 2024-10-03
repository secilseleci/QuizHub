using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

public class UserQuizCardsViewComponent : ViewComponent
{
    private readonly IServiceManager _serviceManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper; // AutoMapper kullanarak dönüşüm yapacağız

    public UserQuizCardsViewComponent(IServiceManager serviceManager, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _serviceManager = serviceManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(string status)
    {
        // Kullanıcı giriş yapmışsa rollerine göre ayır
        var claimsPrincipal = User as ClaimsPrincipal;
        var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier); // UserId'yi alıyoruz
        var user = await _userManager.FindByIdAsync(userId);  // ApplicationUser kullanıyoruz

        if (user == null)
        {
            return Content("User not found"); // Eğer kullanıcı bulunamazsa
        }

        var quizzes = new List<UserQuizInfo>(); // Quiz listesi

        switch (status)
        {
            case "Pending":
                var allQuizzes = _serviceManager.QuizService.GetQuizzesWithDepartments(false).ToList();
                var solvedQuizIds = _serviceManager.UserQuizInfoService
                    .GetUserQuizInfoByUserId(userId, false)
                    .Select(uqi => uqi.QuizId).ToList();

                var departmentId = user.DepartmentId;

                quizzes = allQuizzes
                    .Where(q => q.ShowCase && q.Departments.Any(d => d.DepartmentId == departmentId) && !solvedQuizIds.Contains(q.QuizId))
                    .Select(q => new UserQuizInfo { Quiz = q }).ToList();
                break;

            case "Completed":
                quizzes = _serviceManager.UserQuizInfoService
                    .GetUserQuizInfoByUserId(userId, false)
                    .Where(uqi => uqi.IsCompleted && (uqi.Score >= 60 || uqi.IsSuccessful))
                    .ToList();
                break;

            case "Retake":
                quizzes = _serviceManager.UserQuizInfoService
                    .GetUserQuizInfoByUserId(userId, false)
                    .Where(uqi => uqi.IsCompleted && (uqi.Score < 60 || (uqi.Score < 100 && uqi.IsSuccessful)))
                    .ToList();
                break;

            case "Continue":
                quizzes = _serviceManager.UserQuizInfoService
                    .GetUserQuizInfoByUserId(userId, false)
                    .Where(uqi => !uqi.IsCompleted)
                    .ToList();
                break;

            default:
                quizzes = new List<UserQuizInfo>();
                break;
        }

        // UserQuizInfo modelini UserQuizCardDto modeline dönüştür
        var quizCardDtos = quizzes.Select(q => new UserQuizCardDto
        {
            QuizId = q.Quiz.QuizId,
            QuizTitle = q.Quiz.Title,
            QuestionCount = q.Quiz.QuestionCount,
            Score = q.Score,
            CompletedAt = q.CompletedAt,
            CanRetake = q.Score < 100,
            Status = status
        }).ToList();

        return View("UserQuizCards", quizCardDtos);
    }
}
