using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
      
        public interface IUserQuizInfoService
        {
            UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);
            IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges);
            void AssignQuizToUsers(int quizId, List<string> userIds);
        void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo);

    }
}
 
