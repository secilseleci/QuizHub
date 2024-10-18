using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserAnswerTempManager:IUserAnswerTempService
    {
        private readonly IRepositoryManager _manager;

        public UserAnswerTempManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public void CreateTempAnswer(UserAnswerTemp userAnswerTemp)
        {
            _manager.UserAnswerTemp.CreateTempAnswer(userAnswerTemp);
            _manager.Save();
        }

        public void DeleteTempAnswer(UserAnswerTemp userAnswerTemp)
        {
            _manager.UserAnswerTemp.DeleteTempAnswer(userAnswerTemp);
            _manager.Save();
        }

        public void UpdateTempAnswer(UserAnswerTemp entity)
        {
            _manager.UserAnswerTemp.UpdateTempAnswer(entity);
            _manager.Save();

        }
        public UserAnswerTemp? GetOneTempAnswer(int userQuizInfoTempId, int questionId, bool trackChanges)
        {
          return  _manager.UserAnswerTemp.GetOneTempAnswer(userQuizInfoTempId, questionId, trackChanges);        
        }

        public IEnumerable<UserAnswerTemp> GetTempAnswersByTempInfoId(int userQuizInfoTempId, bool trackChanges)
        {
            return _manager.UserAnswerTemp.GetTempAnswersByTempInfoId(userQuizInfoTempId, trackChanges);
        }
    }
}
