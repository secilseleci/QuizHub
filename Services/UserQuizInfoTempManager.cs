using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserQuizInfoTempManager: IUserQuizInfoTempService
    {
        private readonly IRepositoryManager _manager;
        public UserQuizInfoTempManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public void CreateTempInfo(UserQuizInfoTemp userQuizInfoTemp)
        {
            _manager.UserQuizInfoTemp.CreateTempInfo(userQuizInfoTemp);
            _manager.Save();
        }

      
        public void RemoveTempInfo(UserQuizInfoTemp userQuizInfoTemp)
        {
            _manager.UserQuizInfoTemp.RemoveTempInfo(userQuizInfoTemp);
            _manager.Save();
        }

        public void UpdateTempInfo(UserQuizInfoTemp entity)
        {
            _manager.UserQuizInfoTemp.UpdateTempInfo(entity);
            _manager.Save();
        }
        public UserQuizInfoTemp GetTempInfoById(int userQuizInfoTempId, bool trackChanges)
        {
            return _manager.UserQuizInfoTemp.GetTempInfoById(userQuizInfoTempId, trackChanges);
        }

        public UserQuizInfoTemp? GetTempInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            return _manager.UserQuizInfoTemp.GetTempInfoByQuizIdAndUserId(quizId, userId, trackChanges);        
        }


        public IQueryable<UserQuizInfoTemp> GetTempInfoByUserId(string userId, bool trackChanges)
        {
            return _manager.UserQuizInfoTemp.GetTempInfoByUserId(userId, trackChanges);
        }
        public UserQuizInfoTemp GetOneTempInfoByUserId(string userId, bool trackChanges)
        {
            return _manager.UserQuizInfoTemp.GetOneTempInfoByUserId(userId, trackChanges);
        }

        public IQueryable<UserQuizInfoTemp> GetIncompleteQuizzesByUserId(string userId, bool trackChanges)
        {
            return _manager.UserQuizInfoTemp.GetIncompleteQuizzesByUserId(userId,trackChanges);
        }
    }
}
