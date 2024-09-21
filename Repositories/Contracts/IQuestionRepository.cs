using Entities.Models;
using System.Linq;

namespace Repositories.Contracts
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        IQueryable<Question> GetAllQuestions(bool trackChanges);
        IQueryable<Question> GetQuestionsByQuizId(int quizId, bool trackChanges);
        Question? GetOneQuestion(int id, bool trackChanges);
       
        void CreateOneQuestion(Question question);
        void UpdateOneQuestion(Question entity);
        void DeleteOneQuestion(Question question);
    

        Question? GetOneQuestionWithOptions(int id, bool trackChanges);
    }
}
 
 