using Entities.Exeptions;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserQuizInfoService
    {
        Task<ResultGeneric<UserQuizInfo>> GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);
        Task<ResultGeneric<IEnumerable<UserQuizInfo>>> GetUserQuizInfoByUserId(string userId, bool trackChanges);
        Task<Result> AssignQuizToUsers(int quizId, List<string> userIds);
        Task<ResultGeneric<UserQuizInfo>> CreateOneUserQuizInfo(UserQuizInfo userQuizInfo);  // ResultGeneric<UserQuizInfo> olarak güncellendi
        Task<ResultGeneric<UserQuizInfo>> UpdateOneUserQuizInfo(UserQuizInfo userQuizInfo);  // ResultGeneric<UserQuizInfo> olarak güncellendi
        Task<ResultGeneric<UserQuizInfo>> GetUserQuizInfoById(int userQuizInfoId, bool trackChanges);
        Task<ResultGeneric<UserQuizInfo>> ProcessQuiz(int quizId, string userId);
        Task<ResultGeneric<UserQuizInfo>> SaveQuiz(UserQuizInfoTemp userQuizInfoTemp, IEnumerable<UserAnswerTemp> userAnswersTemp);
        Task<ResultGeneric<UserQuizInfo>> UpdateQuiz(UserQuizInfo existingQuizInfo, IEnumerable<UserAnswerTemp> userAnswersTemp, UserQuizInfoTemp userQuizInfoTemp);
        Task<ResultGeneric<IEnumerable<UserQuizInfo>>> GetRetakeableQuizzesByUserId(string userId, bool trackChanges);
        Task<ResultGeneric<IEnumerable<UserQuizInfo>>> GetCompletedQuizzesByUserId(string userId, bool trackChanges);
    }
}
