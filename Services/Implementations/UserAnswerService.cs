using Repositories.Contracts;
using Services.Contracts;
using Entities.Models;
using Entities.Exeptions;

namespace Services.Implementations
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IRepositoryManager _manager;

        public UserAnswerService(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<Result> CreateUserAnswer(UserAnswer userAnswer)
        {
            await _manager.UserAnswer.CreateUserAnswerAsync(userAnswer);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Kullanıcı cevabı kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }
            return Result.Ok();
        }

        public async Task<Result> UpdateUserAnswer(UserAnswer userAnswer)
        {
            await _manager.UserAnswer.UpdateUserAnswerAsync(userAnswer);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Kullanıcı cevabı güncellenemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }
            return Result.Ok();
        }

        public async Task<ResultGeneric<IEnumerable<UserAnswer>>> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges)
        {
            var userAnswers = await _manager.UserAnswer.GetUserAnswersByQuizInfoIdAsync(userQuizInfoId, trackChanges);
            if (!userAnswers.Any())
            {
                return ResultGeneric<IEnumerable<UserAnswer>>.Fail("Kullanıcı cevapları bulunamadı.", "Bu quiz için kullanıcı cevabı mevcut değil.");
            }

            return ResultGeneric<IEnumerable<UserAnswer>>.Ok(userAnswers);
        }

        public async Task<ResultGeneric<UserAnswer>> GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges)
        {
            var userAnswer = await _manager.UserAnswer.GetUserAnswerAsync(userQuizInfoId, questionId, trackChanges);
            if (userAnswer == null)
            {
                return ResultGeneric<UserAnswer>.Fail("Kullanıcı cevabı bulunamadı.", "Belirtilen quiz ve soru için kullanıcı cevabı mevcut değil.");
            }

            return ResultGeneric<UserAnswer>.Ok(userAnswer);
        }
    }
}
