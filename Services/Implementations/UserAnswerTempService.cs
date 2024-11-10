using Entities.Exeptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Implementations
{
    public class UserAnswerTempService : IUserAnswerTempService
    {
        private readonly IRepositoryManager _manager;

        public UserAnswerTempService(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<Result> CreateTempAnswer(UserAnswerTemp userAnswerTemp)
        {
            await _manager.UserAnswerTemp.CreateTempAnswerAsync(userAnswerTemp);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Geçici kullanıcı cevabı kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }
            return Result.Ok();
        }

        public async Task<Result> UpdateTempAnswer(UserAnswerTemp entity)
        {
            await _manager.UserAnswerTemp.UpdateTempAnswerAsync(entity);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Geçici kullanıcı cevabı güncellenemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }
            return Result.Ok();
        }

        public async Task<Result> DeleteTempAnswer(UserAnswerTemp userAnswerTemp)
        {
            await _manager.UserAnswerTemp.DeleteTempAnswerAsync(userAnswerTemp);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Geçici kullanıcı cevabı silinemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }
            return Result.Ok();
        }

        public async Task<ResultGeneric<IEnumerable<UserAnswerTemp>>> GetTempAnswersByTempInfoId(int userQuizInfoTempId, bool trackChanges)
        {
            var userAnswers = await _manager.UserAnswerTemp.GetTempAnswersByTempInfoIdAsync(userQuizInfoTempId, trackChanges);
            if (!userAnswers.Any())
            {
                return ResultGeneric<IEnumerable<UserAnswerTemp>>.Fail("Geçici kullanıcı cevapları bulunamadı.", "Bu quiz bilgisi için geçici cevap mevcut değil.");
            }

            return ResultGeneric<IEnumerable<UserAnswerTemp>>.Ok(userAnswers);
        }

        public async Task<ResultGeneric<UserAnswerTemp>> GetOneTempAnswer(int userQuizInfoTempId, int questionId, bool trackChanges)
        {
            var userAnswerTemp = await _manager.UserAnswerTemp.GetOneTempAnswerAsync(userQuizInfoTempId, questionId, trackChanges);
            if (userAnswerTemp == null)
            {
                return ResultGeneric<UserAnswerTemp>.Fail("Geçici kullanıcı cevabı bulunamadı.", "Belirtilen quiz ve soru için geçici kullanıcı cevabı mevcut değil.");
            }

            return ResultGeneric<UserAnswerTemp>.Ok(userAnswerTemp);
        }
    }
}
