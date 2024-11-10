using Entities.Models;
using Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations
{
    public class UserAnswerRepository : RepositoryBase<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task CreateUserAnswerAsync(UserAnswer userAnswer) => CreateAsync(userAnswer);
        public async Task UpdateUserAnswerAsync(UserAnswer entity) => UpdateAsync(entity);

        public async Task<IEnumerable<UserAnswer>> GetUserAnswersByQuizInfoIdAsync(int userQuizInfoId, bool trackChanges)
        {
             var query = await FindAllAsync(trackChanges);
            return await query
                .Where(ua => ua.UserQuizInfoId == userQuizInfoId)
                .ToListAsync();
        }

  
         public async Task<UserAnswer?> GetUserAnswerAsync(int userQuizInfoId, int questionId, bool trackChanges)
        {
            return await FindByConditionAsync(
               ua => ua.UserQuizInfoId == userQuizInfoId && ua.QuestionId == questionId,
               trackChanges);
           
        }
}
 }