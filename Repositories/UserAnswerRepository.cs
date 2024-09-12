using Entities.Models;
using Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class UserAnswerRepository : RepositoryBase<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        // Yeni bir UserAnswer oluşturma
        public void CreateUserAnswer(UserAnswer userAnswer)
        {
            Create(userAnswer);  // RepositoryBase sınıfındaki Create metodunu kullanıyoruz
        }

        public IEnumerable<UserAnswer> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges)
        {
            return FindAll(trackChanges)
                   .Where(ua => ua.UserQuizInfoId == userQuizInfoId)
                   .ToList();
        }


        // Belirli bir UserAnswer'ı UserQuizInfoId ve QuestionId'ye göre getirme
        public UserAnswer? GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges)
        {
            return FindByCondition(ua => ua.UserQuizInfoId == userQuizInfoId && ua.QuestionId == questionId, trackChanges);
        }
    }
}
