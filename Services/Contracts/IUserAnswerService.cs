using Entities.Exeptions;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserAnswerService
    {
        Task<Result> UpdateUserAnswer(UserAnswer userAnswer);
        Task<Result> CreateUserAnswer(UserAnswer userAnswer);
        Task<ResultGeneric<IEnumerable<UserAnswer>>> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges);
        Task<ResultGeneric<UserAnswer>> GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges);
    }
}

