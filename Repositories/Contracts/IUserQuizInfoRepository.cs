using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUserQuizInfoRepository : IRepositoryBase<UserQuizInfo>
    {
        Task CreateOneUserQuizInfoAsync(UserQuizInfo userQuizInfo);
        Task UpdateOneUserQuizInfoAsync(UserQuizInfo entity);
        Task RemoveOneUserQuizInfoAsync(UserQuizInfo userQuizInfo);

        Task<UserQuizInfo?> GetUserQuizInfoByIdAsync(int userQuizInfoId, bool trackChanges);
        Task<UserQuizInfo?> GetUserQuizInfoByQuizIdAndUserIdAsync(int quizId, string userId, bool trackChanges);

        Task<IEnumerable<UserQuizInfo>> GetUserQuizInfoByUserIdAsync(string userId, bool trackChanges);
        Task<IEnumerable<UserQuizInfo>> GetRetakeableQuizzesByUserIdAsync(string userId, bool trackChanges);
        Task<IEnumerable<UserQuizInfo>> GetCompletedQuizzesByUserIdAsync(string userId, bool trackChanges);
    }
}
