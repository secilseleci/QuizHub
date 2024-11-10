using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUserAnswerTempRepository : IRepositoryBase<UserAnswerTemp>
    {
        Task CreateTempAnswerAsync(UserAnswerTemp userAnswerTemp);
        Task UpdateTempAnswerAsync(UserAnswerTemp entity);
        Task DeleteTempAnswerAsync(UserAnswerTemp userAnswerTemp);
        // QuizInfoId'ye göre tüm UserAnswer'ları alma
        Task<IEnumerable<UserAnswerTemp>> GetTempAnswersByTempInfoIdAsync(int userQuizInfoTempId, bool trackChanges);

        // Belirli bir UserAnswer'ı alma (InfoId ve QuestionId ile)
        Task<UserAnswerTemp?> GetOneTempAnswerAsync(int userQuizInfoTempId, int questionId, bool trackChanges);
    }
}
