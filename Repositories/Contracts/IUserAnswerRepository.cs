using Entities.Models;
using System;
using System.Collections.Generic;
 

namespace Repositories.Contracts
{
    public interface IUserAnswerRepository : IRepositoryBase<UserAnswer>
    {
        Task  CreateUserAnswerAsync(UserAnswer userAnswer);
        Task UpdateUserAnswerAsync(UserAnswer userAnswer);

        // QuizInfoId'ye göre tüm UserAnswer'ları alma
        Task<IEnumerable<UserAnswer>> GetUserAnswersByQuizInfoIdAsync(int userQuizInfoId, bool trackChanges);

        // Belirli bir UserAnswer'ı alma (UserQuizInfoId ve QuestionId e göre)
        Task<UserAnswer?> GetUserAnswerAsync(int userQuizInfoId, int questionId, bool trackChanges);
    }

}
