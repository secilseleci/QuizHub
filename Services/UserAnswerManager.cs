using Repositories.Contracts;
using Services.Contracts;
using Entities.Models;

namespace Services
{
    public class UserAnswerManager : IUserAnswerService
    {
        private readonly IRepositoryManager _repository;

        public UserAnswerManager(IRepositoryManager repository)
        {
            _repository = repository;
        }

        // Yeni bir kullanıcı cevabı oluşturma
        public void CreateUserAnswer(int quizId, int questionId, string userId, int selectedOptionId)
        {
            var userQuizInfo = _repository.UserQuizInfo.FindByCondition(uqi => uqi.QuizId == quizId && uqi.UserId == userId, trackChanges: false);
            if (userQuizInfo == null)
            {
                throw new Exception("UserQuizInfo not found.");
            }

            var userAnswer = new UserAnswer
            {
                UserQuizInfoId = userQuizInfo.UserQuizInfoId,
                QuestionId = questionId,
                SelectedOptionId = selectedOptionId
            };

            _repository.UserAnswer.Create(userAnswer);
            _repository.Save();
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
    }
}
