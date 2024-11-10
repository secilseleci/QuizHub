using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.Implementations
{
    public class UserQuizInfoRepository : RepositoryBase<UserQuizInfo>, IUserQuizInfoRepository
    {
        public UserQuizInfoRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateOneUserQuizInfoAsync(UserQuizInfo userQuizInfo) => await CreateAsync(userQuizInfo);
        public async Task UpdateOneUserQuizInfoAsync(UserQuizInfo entity) => await UpdateAsync(entity);
        public async Task RemoveOneUserQuizInfoAsync(UserQuizInfo userQuizInfo) => await RemoveAsync(userQuizInfo);

        public async Task<UserQuizInfo?> GetUserQuizInfoByIdAsync(int userQuizInfoId, bool trackChanges) =>
            await FindByConditionAsync(uqi => uqi.UserQuizInfoId == userQuizInfoId, trackChanges);

        public async Task<UserQuizInfo?> GetUserQuizInfoByQuizIdAndUserIdAsync(int quizId, string userId, bool trackChanges)
        {
            return await (trackChanges
                ? _context.UserQuizInfo
                    .SingleOrDefaultAsync(uqi => uqi.QuizId == quizId && uqi.UserId == userId)
                : _context.UserQuizInfo
                    .AsNoTracking()
                    .SingleOrDefaultAsync(uqi => uqi.QuizId == quizId && uqi.UserId == userId));
        }

        public async Task<IEnumerable<UserQuizInfo>> GetUserQuizInfoByUserIdAsync(string userId, bool trackChanges)
        {
            var query = trackChanges
                ? _context.UserQuizInfo.Where(uqi => uqi.UserId == userId)
                : _context.UserQuizInfo.AsNoTracking().Include(uqi => uqi.Quiz).Where(uqi => uqi.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserQuizInfo>> GetRetakeableQuizzesByUserIdAsync(string userId, bool trackChanges)
        {
            var query = await FindAllByConditionAsync(
                uqi => uqi.UserId == userId && uqi.IsCompleted && uqi.Score < 100,
                trackChanges
            );

            return await query.Include(uqi => uqi.Quiz).ToListAsync();
        }

        public async Task<IEnumerable<UserQuizInfo>> GetCompletedQuizzesByUserIdAsync(string userId, bool trackChanges)
        {
            var query = await FindAllByConditionAsync(
                uqi => uqi.UserId == userId && uqi.IsCompleted && uqi.Score >= 60,
                trackChanges
            );

            return await query.Include(uqi => uqi.Quiz).ToListAsync();
        }
    }
}
