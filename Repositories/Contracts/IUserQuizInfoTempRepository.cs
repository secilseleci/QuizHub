using Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserQuizInfoTempRepository : IRepositoryBase<UserQuizInfoTemp>
    {

        Task CreateTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp);

        Task UpdateTempInfoAsync(UserQuizInfoTemp entity);

        Task RemoveTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp);
        Task<UserQuizInfoTemp> GetTempInfoByIdAsync(int userQuizInfoTempId, bool trackChanges);
        Task<UserQuizInfoTemp?> GetTempInfoByQuizIdAndUserIdAsync(int quizId, string userId, bool trackChanges);
        Task<UserQuizInfoTemp> GetOneTempInfoByUserIdAsync(string userId, bool trackChanges);

        Task<IQueryable<UserQuizInfoTemp>> GetTempInfoByUserIdAsync(string userId, bool trackChanges);

        Task<List<UserQuizInfoTemp>> GetIncompleteQuizzesByUserIdAsync(string userId, bool trackChanges);
    }
}
