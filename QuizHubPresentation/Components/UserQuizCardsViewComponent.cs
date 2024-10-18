using AutoMapper;
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
    private readonly IMapper _mapper;

    public UserQuizCardsViewComponent(IServiceManager serviceManager, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _serviceManager = serviceManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(string status)
    {
         var claimsPrincipal = User as ClaimsPrincipal;
        var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Content("User not found");
        }

        var quizCardDtos = new List<UserQuizCardDto>();

        switch (status)
        {
            case "Pending":
                quizCardDtos = GetPendingQuizzes(userId, user.DepartmentId);
                break;

            case "Completed":
                quizCardDtos = GetCompletedQuizzes(userId);
                break;

            case "Retake":
                quizCardDtos = GetRetakeQuizzes(userId);
                break;

            case "Continue":
                quizCardDtos = GetContinueQuizzes(userId);
                break;

            default:
                quizCardDtos = new List<UserQuizCardDto>();
                break;
        }
        foreach (var quizCard in quizCardDtos)
        {
            quizCard.Status = status;  
        }
        return View("UserQuizCards", quizCardDtos);
    }

    private List<UserQuizCardDto> GetPendingQuizzes(string userId, int departmentId)
    {
        var pendingQuizzes = _serviceManager.QuizService
            .GetPendingQuizzesForUser(userId, trackChanges: false);
        return _mapper.Map<List<UserQuizCardDto>>(pendingQuizzes);
    }


    private List<UserQuizCardDto> GetCompletedQuizzes(string userId)
    {
        var completedQuizzes = _serviceManager.UserQuizInfoService
            .GetCompletedQuizzesByUserId(userId, trackChanges: false)
            .ToList();

        return _mapper.Map<List<UserQuizCardDto>>(completedQuizzes);
    }

    private List<UserQuizCardDto> GetRetakeQuizzes(string userId)
    {
        var retakeQuizzes = _serviceManager.UserQuizInfoService
         .GetRetakeableQuizzesByUserId(userId, trackChanges: false)   
         .ToList();

        return _mapper.Map<List<UserQuizCardDto>>(retakeQuizzes);
    }

    private List<UserQuizCardDto> GetContinueQuizzes(string userId)
    {
        var incompleteQuizzes = _serviceManager.UserQuizInfoTempService
            .GetIncompleteQuizzesByUserId(userId, trackChanges: false)
            .ToList();

        return _mapper.Map<List<UserQuizCardDto>>(incompleteQuizzes);
    }
}
