using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
 

namespace Repositories.Implementations
{
    public class UserAnswerTempRepository: RepositoryBase<UserAnswerTemp>, IUserAnswerTempRepository
    {
        public UserAnswerTempRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task  CreateTempAnswerAsync(UserAnswerTemp userAnswerTemp) => await CreateAsync(userAnswerTemp);
        public async Task DeleteTempAnswerAsync(UserAnswerTemp userAnswerTemp) => await RemoveAsync(userAnswerTemp);
        public async Task UpdateTempAnswerAsync(UserAnswerTemp entity) => await UpdateAsync(entity);


        public async Task<UserAnswerTemp?> GetOneTempAnswerAsync(int userQuizInfoTempId, int questionId, bool trackChanges)
        {
            return await FindByConditionAsync(
                  ua => ua.UserQuizInfoTempId == userQuizInfoTempId && ua.QuestionId == questionId,
                  trackChanges);
        }

        public async Task< IEnumerable<UserAnswerTemp>> GetTempAnswersByTempInfoIdAsync(int userQuizInfoTempId, bool trackChanges)
        {
            var query = await FindAllByConditionAsync(
            ua => ua.UserQuizInfoTempId == userQuizInfoTempId,
            trackChanges);
    

            return await query.ToListAsync();

        }


    }
}
