using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Repositories.Extensions;

namespace Repositories
{
    public class QuizRepository : RepositoryBase<Quiz>, IQuizRepository
    {
        public QuizRepository(RepositoryContext context) : base(context) 
    
        { 
    
        }

        public void CreateOneQuiz(Quiz quiz) => Create(quiz);
        public void UpdateOneQuiz(Quiz entity) => Update(entity);
        public void DeleteOneQuiz(Quiz quiz)=>Remove(quiz);

        public IQueryable<Quiz> GetAllQuizzes(bool trackChanges) => FindAll(trackChanges);

        public Quiz? GetOneQuiz(int id, bool trackChanges)
        {
            return FindByCondition(p => p.QuizId.Equals(id), trackChanges);
        }
        public IQueryable<Quiz> GetShowCaseQuizzes(bool trackChanges)
        {
            return FindAll(trackChanges)
            .Where(p => p.ShowCase == true);


        }

        public IQueryable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q)
        {
            return _context
            .Quizzes
            .FilteredBySearchTerm(q.SearchTerm)
            .ToPaginate(q.PageNumber, q.PageSize);
        }

        
    }
}
