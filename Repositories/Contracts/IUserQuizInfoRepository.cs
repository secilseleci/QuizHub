using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUserQuizInfoRepository : IRepositoryBase<UserQuizInfo>
    {
        UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);
        void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo);
        UserQuizInfo GetUserQuizInfoById(int userQuizInfoId, bool trackChanges);

    }

}
