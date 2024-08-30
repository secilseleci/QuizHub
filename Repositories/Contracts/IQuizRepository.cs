using Entities.Models;
using Entities.RequestParameters;

namespace Repositories.Contracts
{
    public interface IQuizRepository:IRepositoryBase<Quiz>
    {

        IQueryable<Quiz> GetAllQuizzes(bool trackChanges);
        IQueryable<Quiz> GetShowCaseQuizzes(bool trackChanges);

        IQueryable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q);

        Quiz? GetOneQuiz(int id, bool trackChanges);
        void CreateOneQuiz(Quiz quiz);
        void UpdateOneQuiz(Quiz entity);
        void DeleteOneQuiz(Quiz quiz);

    }
}
