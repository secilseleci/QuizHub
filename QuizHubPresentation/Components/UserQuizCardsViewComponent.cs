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
                quizCardDtos = await GetPendingQuizzesAsync(userId, user.DepartmentId);
                break;

            case "Completed":
                quizCardDtos = await GetCompletedQuizzesAsync(userId);
                break;

            case "Retake":
                quizCardDtos = await GetRetakeQuizzesAsync(userId);
                break;

            case "Continue":
                quizCardDtos = await GetContinueQuizzesAsync(userId);
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

    private async Task<List<UserQuizCardDto>> GetPendingQuizzesAsync(string userId, int departmentId)
    {
        var pendingQuizzesResult = await _serviceManager.QuizService
            .GetPendingQuizzesForUser(userId, trackChanges: false);

        if (!pendingQuizzesResult.IsSuccess || pendingQuizzesResult.Data == null)
            return new List<UserQuizCardDto>();

        return _mapper.Map<List<UserQuizCardDto>>(pendingQuizzesResult.Data);
    }

    private async Task<List<UserQuizCardDto>> GetCompletedQuizzesAsync(string userId)
    {
        var completedQuizzesResult = await _serviceManager.UserQuizInfoService
            .GetCompletedQuizzesByUserId(userId, trackChanges: false);

        if (!completedQuizzesResult.IsSuccess || completedQuizzesResult.Data == null)
            return new List<UserQuizCardDto>();

        return _mapper.Map<List<UserQuizCardDto>>(completedQuizzesResult.Data);
    }

    private async Task<List<UserQuizCardDto>> GetRetakeQuizzesAsync(string userId)
    {
        var retakeQuizzesResult = await _serviceManager.UserQuizInfoService
            .GetRetakeableQuizzesByUserId(userId, trackChanges: false);

        if (!retakeQuizzesResult.IsSuccess || retakeQuizzesResult.Data == null)
            return new List<UserQuizCardDto>();

        return _mapper.Map<List<UserQuizCardDto>>(retakeQuizzesResult.Data);
    }

    private async Task<List<UserQuizCardDto>> GetContinueQuizzesAsync(string userId)
    {
        var incompleteQuizzesResult = await _serviceManager.UserQuizInfoTempService
            .GetIncompleteQuizzesByUserIdAsync(userId, trackChanges: false);

        if (!incompleteQuizzesResult.IsSuccess || incompleteQuizzesResult.Data == null)
        return new List<UserQuizCardDto>();

        return _mapper.Map<List<UserQuizCardDto>>(incompleteQuizzesResult.Data);
    }
}
