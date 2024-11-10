using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
 
namespace Repositories.Implementations
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
 
        public QuestionRepository(RepositoryContext context) : base(context) { 
         }

       
        public async Task CreateOneQuestionAsync(Question question) => CreateAsync(question);
        public async Task UpdateOneQuestionAsync(Question entity) => UpdateAsync(entity);
        public async Task DeleteOneQuestionAsync(Question question) => RemoveAsync(question);


        public async Task<IQueryable<Question>> GetAllQuestionsAsync(bool trackChanges) => 
            await FindAllAsync(trackChanges);


        public async Task<Question?> GetOneQuestionAsync(int id, bool trackChanges) =>
           await FindByConditionAsync(q => q.QuestionId == id, trackChanges);
       

        public async Task<IQueryable<Question>> GetQuestionsByQuizIdAsync(int quizId, bool trackChanges)=>
            await FindAllByConditionAsync(q => q.QuizId == quizId, trackChanges);
       
        public async Task<Question?> GetOneQuestionWithOptionsAsync(int id, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                        .Include(q => q.Options)
                        .Where(q => q.QuestionId == id)
                        .SingleOrDefaultAsync();
     
        }

        public async Task<Question?> GetNextQuestionByQuizIdAsync(int quizId, int currentQuestionOrder, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                        .Include(q => q.Options)  // Options dahil ediliyor
                        .Where(q => q.QuizId == quizId && q.Order > currentQuestionOrder)
                        .OrderBy(q => q.Order)
                        .FirstOrDefaultAsync();
        }

        public async Task<Question?> GetLastQuestionByQuizIdAsync(int quizId, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                .Include(q => q.Options)
                .Where(q => q.QuizId == quizId)
                .OrderByDescending(q => q.Order)
                .FirstOrDefaultAsync();
        }
    }
}
