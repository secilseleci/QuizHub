using Repositories.Contracts;
using Services.Contracts;
using Entities.Models;
using Entities.Dtos;

namespace Services
{
    public class UserAnswerManager : IUserAnswerService
    {
        private readonly IRepositoryManager _manager;

        public UserAnswerManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public void UpdateUserAnswer(UserAnswer userAnswer)
        {
            _manager.UserAnswer.UpdateUserAnswer(userAnswer);
            _manager.Save();// Update metodu burada
        }

        // Quiz'e ait tüm kullanıcı cevaplarını getir
        public IEnumerable<UserAnswer> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges)
        {
            return _manager.UserAnswer.GetUserAnswersByQuizInfoId(userQuizInfoId, trackChanges);
        }

        // Belirli bir soruya ait kullanıcı cevabını getir
        public UserAnswer GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges)
        {
            return _manager.UserAnswer.GetUserAnswer(userQuizInfoId, questionId, trackChanges);
        }

        public void CreateUserAnswer(UserAnswer userAnswer)
        {
            _manager.UserAnswer.CreateUserAnswer(userAnswer);
            _manager.Save(); // Repository'de işlem yapıldıktan sonra Save çağrılmalı
        }
    }
}
 
