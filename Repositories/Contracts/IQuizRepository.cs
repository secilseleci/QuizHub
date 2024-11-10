using Entities.Models;
using Entities.RequestParameters;

namespace Repositories.Contracts
{
    public interface IQuizRepository : IRepositoryBase<Quiz>
    {
        Task<IQueryable<Quiz>> GetAllQuizzesAsync(bool trackChanges);
        Task<IQueryable<Quiz>> GetShowCaseQuizzesAsync(bool trackChanges);

        Task CreateOneQuizAsync(Quiz quiz);
        Task UpdateOneQuizAsync(Quiz entity);
        Task DeleteOneQuizAsync(Quiz quiz);

        Task<Quiz?> GetOneQuizAsync(int id, bool trackChanges);
        Task<Quiz?> GetQuizWithDetailsAsync(int quizId, bool trackChanges);
        Task<Quiz?> GetQuizWithDepartmentsAsync(int quizId, bool trackChanges);
        Task<IEnumerable<Quiz>> GetQuizzesWithDepartmentsAsync(bool trackChanges);

        Task<IEnumerable<Quiz>> GetQuizzesByDepartmentIdAsync(int departmentId, bool trackChanges);
    }
}