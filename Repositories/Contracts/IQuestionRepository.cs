using Entities.Models;
 
namespace Repositories.Contracts
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task CreateOneQuestionAsync(Question question);
        Task UpdateOneQuestionAsync(Question entity);
        Task DeleteOneQuestionAsync(Question question);
        Task<IQueryable<Question>> GetAllQuestionsAsync(bool trackChanges);
        Task<IQueryable<Question>> GetQuestionsByQuizIdAsync(int quizId, bool trackChanges);
        Task<Question?> GetOneQuestionAsync(int id, bool trackChanges);
        Task<Question?> GetOneQuestionWithOptionsAsync(int id, bool trackChanges);
        Task<Question?> GetNextQuestionByQuizIdAsync(int quizId, int currentQuestionOrder, bool trackChanges);
        Task<Question?> GetLastQuestionByQuizIdAsync(int quizId, bool trackChanges);

    }
}
 
 