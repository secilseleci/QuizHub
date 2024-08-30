using Entities.Models;

namespace Repositories.Extensions
{
    public static class QuizRepositoryExtension
    {
        public static IQueryable<Quiz> FilteredBySearchTerm(this IQueryable<Quiz> quizzes,
            String? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return quizzes;
            else
                return quizzes.Where(q => q.Title.ToLower()
                .Contains(searchTerm.ToLower())
                );
        }


        public static IQueryable<Quiz> ToPaginate(this IQueryable<Quiz> quizzes,
           int pageNumber, int pageSize)
        {
            return quizzes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
