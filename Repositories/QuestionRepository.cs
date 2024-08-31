using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq;

namespace Repositories
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(RepositoryContext context) : base(context) { }


        public void CreateOneQuestion(Question question) => Create(question);
        public void UpdateOneQuestion(Question entity) => Update(entity);
        public void DeleteOneQuestion(Question question) => Remove(question);


        public IQueryable<Question> GetAllQuestions(bool trackChanges) => FindAll(trackChanges);


        public Question? GetOneQuestion(int id, bool trackChanges)
        {
            return FindByCondition(q => q.QuestionId == id, trackChanges);
        }

        public IQueryable<Question> GetQuestionsByQuizId(int quizId, bool trackChanges)
        {
            return FindAll(trackChanges).Where(q => q.QuizId == quizId);
        }

        public Question? GetOneQuestionWithOptions(int id, bool trackChanges)
        {
            return _context.Questions
                    .Where(q => q.QuestionId == id)
                    .Include(q => q.Options)
                    .SingleOrDefault();
        }
 

    }
}
