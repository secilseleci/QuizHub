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
        // Bir kullanıcının belirli bir quize ait sonuçları getir
        UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);
        void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo);
        UserQuizInfo GetUserQuizInfoById(int userQuizInfoId, bool trackChanges);

        // Bir kullanıcının tüm quiz sonuçlarını getir 
        IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges);
    }

}
