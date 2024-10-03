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
        void UpdateUserAnswer(UserAnswer userAnswer);  // Update metodu burada

        void CreateUserAnswer(UserAnswer userAnswer);
        IEnumerable<UserAnswer> GetUserAnswersByQuizInfoId(int userQuizInfoId, bool trackChanges);
        UserAnswer GetUserAnswer(int userQuizInfoId, int questionId, bool trackChanges);
    }
}

