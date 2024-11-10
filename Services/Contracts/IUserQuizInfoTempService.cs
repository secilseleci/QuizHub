using Entities.Exeptions;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserQuizInfoTempService
    {
        Task<ResultGeneric<UserQuizInfoTemp>> GetTempInfoByQuizIdAndUserIdAsync(int quizId, string userId, bool trackChanges);

        Task<ResultGeneric<IEnumerable<UserQuizInfoTemp>>> GetTempInfoByUserIdAsync(string userId, bool trackChanges);

        Task<ResultGeneric<IEnumerable<UserQuizInfoTemp>>> GetIncompleteQuizzesByUserIdAsync(string userId, bool trackChanges);

        Task<Result> CreateTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp);

        Task<Result> UpdateTempInfoAsync(UserQuizInfoTemp entity);

        Task<Result> RemoveTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp);

        Task<ResultGeneric<UserQuizInfoTemp>> GetTempInfoByIdAsync(int userQuizInfoTempId, bool trackChanges);

        Task<ResultGeneric<UserQuizInfoTemp>> GetOneTempInfoByUserIdAsync(string userId, bool trackChanges);
    }
}
