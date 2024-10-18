using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IUserAnswerTempService
    {
        
    void CreateTempAnswer(UserAnswerTemp userAnswerTemp);
    void UpdateTempAnswer(UserAnswerTemp entity);
    void DeleteTempAnswer(UserAnswerTemp userAnswerTemp);
    // QuizInfoId'ye göre tüm UserAnswer'ları alma
    IEnumerable<UserAnswerTemp> GetTempAnswersByTempInfoId(int userQuizInfoTempId, bool trackChanges);

    // Belirli bir UserAnswer'ı alma (InfoId ve QuestionId ile)
    UserAnswerTemp? GetOneTempAnswer(int userQuizInfoTempId, int questionId, bool trackChanges);
}
}
