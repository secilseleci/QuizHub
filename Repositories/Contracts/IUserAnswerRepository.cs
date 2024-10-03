using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUserAnswerRepository : IRepositoryBase<UserAnswer>
    {
        void CreateUserAnswer(UserAnswer userAnswer);
        void UpdateUserAnswer(UserAnswer userAnswer);  // Update metodu burada

        // QuizInfoId'ye göre tüm UserAnswer'ları alma
        IEnumerable<UserAnswer> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges);

        // Belirli bir UserAnswer'ı alma (UserQuizInfoId ve QuestionId ile)
        UserAnswer? GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges);
    }

}
