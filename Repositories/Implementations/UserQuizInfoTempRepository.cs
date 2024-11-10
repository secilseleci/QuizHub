using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.Implementations
{
    public class UserQuizInfoTempRepository : RepositoryBase<UserQuizInfoTemp>, IUserQuizInfoTempRepository
    {
        public UserQuizInfoTempRepository(RepositoryContext context) : base(context) { }

        public async Task CreateTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp) => await CreateAsync(userQuizInfoTemp);

        public async Task UpdateTempInfoAsync(UserQuizInfoTemp entity) => await UpdateAsync(entity);

        public async Task RemoveTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp) => await RemoveAsync(userQuizInfoTemp);

        public async Task<UserQuizInfoTemp?> GetTempInfoByIdAsync(int userQuizInfoTempId, bool trackChanges)
        {
            return await FindByConditionAsync(
                tempInfo => tempInfo.UserQuizInfoTempId == userQuizInfoTempId,
                trackChanges
            );
        }

        public async Task<UserQuizInfoTemp?> GetTempInfoByQuizIdAndUserIdAsync(int quizId, string userId, bool trackChanges)
        {
            return await FindByConditionAsync(
                tempInfo => tempInfo.QuizId == quizId && tempInfo.UserId == userId,
                trackChanges
            );
        }

        public async Task<UserQuizInfoTemp?> GetOneTempInfoByUserIdAsync(string userId, bool trackChanges)
        {
            return await FindByConditionAsync(
                tempInfo => tempInfo.UserId == userId,
                trackChanges
            );
        }

        public async Task<IQueryable<UserQuizInfoTemp>> GetTempInfoByUserIdAsync(string userId, bool trackChanges)
        {
            return await FindAllByConditionAsync(
                tempInfo => tempInfo.UserId == userId,
                trackChanges
            );
        }
        public async Task<List<UserQuizInfoTemp>> GetIncompleteQuizzesByUserIdAsync(string userId, bool trackChanges)
        {
            var query = await FindAllByConditionAsync(
                tempInfo => tempInfo.UserId == userId && !tempInfo.IsCompleted,
                trackChanges
            );

            return await query
                .Include(tempInfo => tempInfo.Quiz)
                .Select(tempInfo => new UserQuizInfoTemp
                {
                    UserQuizInfoTempId = tempInfo.UserQuizInfoTempId,
                    UserId = tempInfo.UserId,
                    QuizId = tempInfo.QuizId,
                    IsCompleted = tempInfo.IsCompleted,
                    Score = tempInfo.Score,
                    Quiz = new Quiz
                    {
                        QuizId = tempInfo.Quiz.QuizId,
                        Title = tempInfo.Quiz.Title // Sadece QuizTitle çekiliyor
                    }
                }).ToListAsync();
        }
 
    }
}
