using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUserQuizInfoTempRepository : IRepositoryBase<UserQuizInfoTemp>
    {
        UserQuizInfoTemp? GetTempInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);

        IQueryable<UserQuizInfoTemp> GetTempInfoByUserId(string userId, bool trackChanges);

        void CreateTempInfo(UserQuizInfoTemp userQuizInfoTemp);

        void UpdateTempInfo(UserQuizInfoTemp entity);

        void RemoveTempInfo(UserQuizInfoTemp userQuizInfoTemp);
        UserQuizInfoTemp GetTempInfoById(int userQuizInfoTempId, bool trackChanges);
        UserQuizInfoTemp GetOneTempInfoByUserId(string userId, bool trackChanges);
        IQueryable<UserQuizInfoTemp> GetIncompleteQuizzesByUserId(string userId, bool trackChanges);
    }
}
