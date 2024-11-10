using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.Implementations
{
    public class QuizRepository : RepositoryBase<Quiz>, IQuizRepository
    {
        public QuizRepository(RepositoryContext context) : base(context) { }

        public async Task CreateOneQuizAsync(Quiz quiz) => await CreateAsync(quiz);

        public async Task UpdateOneQuizAsync(Quiz entity) => await UpdateAsync(entity);

        public async Task DeleteOneQuizAsync(Quiz quiz) => await RemoveAsync(quiz);

        public async Task<IQueryable<Quiz>> GetAllQuizzesAsync(bool trackChanges) =>
            await FindAllAsync(trackChanges);

        public async Task<Quiz?> GetOneQuizAsync(int id, bool trackChanges) =>
            await FindByConditionAsync(q => q.QuizId == id, trackChanges);

        public async Task<IQueryable<Quiz>> GetShowCaseQuizzesAsync(bool trackChanges) =>
            await FindAllByConditionAsync(q => q.ShowCase, trackChanges);

        public async Task<Quiz?> GetQuizWithDetailsAsync(int quizId, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .SingleOrDefaultAsync();
        }

        public async Task<Quiz?> GetQuizWithDepartmentsAsync(int quizId, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Departments)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByDepartmentIdAsync(int departmentId, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                .Where(q => q.Departments.Any(d => d.DepartmentId == departmentId) && q.ShowCase)
                .ToListAsync();

        }
        public async Task<IEnumerable<Quiz>> GetQuizzesWithDepartmentsAsync(bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
                .Include(q => q.Departments)
                .ToListAsync();
        }
    }
}
