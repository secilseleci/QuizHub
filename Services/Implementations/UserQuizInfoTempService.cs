using Entities.Exeptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Implemantations
{
    public class UserQuizInfoTempService : IUserQuizInfoTempService
    {
        private readonly IRepositoryManager _manager;

        public UserQuizInfoTempService(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<Result> CreateTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp)
        {
            await _manager.UserQuizInfoTemp.CreateTempInfoAsync(userQuizInfoTemp);
            var saveResult = await _manager.SaveAsync();
            return saveResult ? Result.Ok() : Result.Fail("Geçici bilgi kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
        }

        public async Task<Result> RemoveTempInfoAsync(UserQuizInfoTemp userQuizInfoTemp)
        {
            await _manager.UserQuizInfoTemp.RemoveTempInfoAsync(userQuizInfoTemp);
            var saveResult = await _manager.SaveAsync();
            return saveResult ? Result.Ok() : Result.Fail("Geçici bilgi silinemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
        }

        public async Task<Result> UpdateTempInfoAsync(UserQuizInfoTemp entity)
        {
            await _manager.UserQuizInfoTemp.UpdateTempInfoAsync(entity);
            var saveResult = await _manager.SaveAsync();
            return saveResult ? Result.Ok() : Result.Fail("Geçici bilgi güncellenemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
        }

        public async Task<ResultGeneric<UserQuizInfoTemp>> GetTempInfoByIdAsync(int userQuizInfoTempId, bool trackChanges)
        {
            var tempInfo = await _manager.UserQuizInfoTemp.GetTempInfoByIdAsync(userQuizInfoTempId, trackChanges);
            return tempInfo != null
                ? ResultGeneric<UserQuizInfoTemp>.Ok(tempInfo)
                : ResultGeneric<UserQuizInfoTemp>.Fail("Geçici bilgi bulunamadı.");
        }

        public async Task<ResultGeneric<UserQuizInfoTemp>> GetTempInfoByQuizIdAndUserIdAsync(int quizId, string userId, bool trackChanges)
        {
            var tempInfo = await _manager.UserQuizInfoTemp.GetTempInfoByQuizIdAndUserIdAsync(quizId, userId, trackChanges);
            return tempInfo != null
                ? ResultGeneric<UserQuizInfoTemp>.Ok(tempInfo)
                : ResultGeneric<UserQuizInfoTemp>.Fail("Geçici bilgi bulunamadı.");
        }

        public async Task<ResultGeneric<IEnumerable<UserQuizInfoTemp>>> GetTempInfoByUserIdAsync(string userId, bool trackChanges)
        {
            var tempInfos = await _manager.UserQuizInfoTemp.GetTempInfoByUserIdAsync(userId, trackChanges);
            return tempInfos.Any()
                ? ResultGeneric<IEnumerable<UserQuizInfoTemp>>.Ok(tempInfos)
                : ResultGeneric<IEnumerable<UserQuizInfoTemp>>.Fail("Geçici bilgi bulunamadı.");
        }

        public async Task<ResultGeneric<UserQuizInfoTemp>> GetOneTempInfoByUserIdAsync(string userId, bool trackChanges)
        {
            var tempInfo = await _manager.UserQuizInfoTemp.GetOneTempInfoByUserIdAsync(userId, trackChanges);
            return tempInfo != null
                ? ResultGeneric<UserQuizInfoTemp>.Ok(tempInfo)
                : ResultGeneric<UserQuizInfoTemp>.Fail("Geçici bilgi bulunamadı.");
        }

        public async Task<ResultGeneric<IEnumerable<UserQuizInfoTemp>>> GetIncompleteQuizzesByUserIdAsync(string userId, bool trackChanges)
        {
            var incompleteQuizzes = await _manager.UserQuizInfoTemp.GetIncompleteQuizzesByUserIdAsync(userId, trackChanges);
            return incompleteQuizzes.Any()
                ? ResultGeneric<IEnumerable<UserQuizInfoTemp>>.Ok(incompleteQuizzes)
                : ResultGeneric<IEnumerable<UserQuizInfoTemp>>.Fail("Tamamlanmamış quiz bulunamadı.");
        }
    }
}
