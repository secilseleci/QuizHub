using Entities.Exeptions;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserAnswerTempService
    {

        Task<Result> CreateTempAnswer(UserAnswerTemp userAnswerTemp);
        Task<Result> UpdateTempAnswer(UserAnswerTemp entity);
        Task<Result> DeleteTempAnswer(UserAnswerTemp userAnswerTemp);
        Task<ResultGeneric<IEnumerable<UserAnswerTemp>>> GetTempAnswersByTempInfoId(int userQuizInfoTempId, bool trackChanges);
        Task<ResultGeneric<UserAnswerTemp>> GetOneTempAnswer(int userQuizInfoTempId, int questionId, bool trackChanges);
    }
}
