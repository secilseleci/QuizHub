using Microsoft.EntityFrameworkCore;

using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Repositories.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Repositories
{
    public class QuizRepository : RepositoryBase<Quiz>, IQuizRepository
    {
        
        public QuizRepository(RepositoryContext context) : base(context)

        {

        }

        public void CreateOneQuiz(Quiz quiz) => Create(quiz);
        public void UpdateOneQuiz(Quiz entity) => Update(entity);
        public void DeleteOneQuiz(Quiz quiz) => Remove(quiz);

        public IQueryable<Quiz> GetAllQuizzes(bool trackChanges) => FindAll(trackChanges);

        public Quiz? GetOneQuiz(int id, bool trackChanges)
        {
            return trackChanges
                   ? _context.Quizzes.SingleOrDefault(q => q.QuizId == id)
                   : _context.Quizzes.AsNoTracking().SingleOrDefault(q => q.QuizId == id);
        }

        public IQueryable<Quiz> GetShowCaseQuizzes(bool trackChanges)
        {
            return FindAllByCondition(q => q.ShowCase == true, trackChanges);
        }

        public IQueryable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q)
        {
            return _context
            .Quizzes
            .ToPaginate(q.PageNumber, q.PageSize);
        }


        public Quiz? GetQuizWithDetails(int quizId, bool trackChanges)
        {
            return FindAll(trackChanges)
                   .Where(q => q.QuizId == quizId)
                   .Include(q => q.Questions)
                   .ThenInclude(q => q.Options)
                   .SingleOrDefault();
        }

        public Quiz GetQuizWithDepartments(int quizId, bool trackChanges)
        {
            var query = trackChanges ?
            _context.Quizzes.Include(q => q.Departments).AsTracking() :
            _context.Quizzes.Include(q => q.Departments).AsNoTracking();

            return query.SingleOrDefault(q => q.QuizId == quizId);
        }
        public IEnumerable<Quiz> GetQuizzesByDepartmentId(int departmentId, bool trackChanges)
        {
            var query = _context.Quizzes.Include(q => q.Departments)
                                        .Where(q => q.Departments.Any(d => d.DepartmentId == departmentId)
                                                 && q.ShowCase); // Aktif quizler (ShowCase)

            return trackChanges ? query.AsTracking().ToList() : query.AsNoTracking().ToList();
        }


    }
}