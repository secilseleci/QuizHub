using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IUserAnswerService
    {
        void CreateUserAnswer(int quizId, int questionId, string userId, int selectedOptionId);
        IEnumerable<UserAnswer> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges);
        UserAnswer GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges);
    }
}

