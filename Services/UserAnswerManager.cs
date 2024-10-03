using Repositories.Contracts;
using Services.Contracts;
using Entities.Models;
using Entities.Dtos;

namespace Services
{
    public class UserAnswerManager : IUserAnswerService
    {
        private readonly IRepositoryManager _repository;

        public UserAnswerManager(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public void UpdateUserAnswer(UserAnswer userAnswer)
        {
            _repository.UserAnswer.UpdateUserAnswer(userAnswer);
            _repository.Save();// Update metodu burada
        }

        // Quiz'e ait tüm kullanıcı cevaplarını getir
        public IEnumerable<UserAnswer> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges)
        {
            return _repository.UserAnswer.GetUserAnswersByQuizInfoId(userQuizInfoId, trackChanges);
        }

        // Belirli bir soruya ait kullanıcı cevabını getir
        public UserAnswer GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges)
        {
            return _repository.UserAnswer.GetUserAnswer(userQuizInfoId, questionId, trackChanges);
        }

        public void CreateUserAnswer(UserAnswer userAnswer)
        {
            _repository.UserAnswer.CreateUserAnswer(userAnswer);
            _repository.Save(); // Repository'de işlem yapıldıktan sonra Save çağrılmalı
        }
    }
}
 
