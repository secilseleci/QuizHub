using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserAnswerTempRepository: RepositoryBase<UserAnswerTemp>, IUserAnswerTempRepository
    {
        public UserAnswerTempRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateTempAnswer(UserAnswerTemp userAnswerTemp) => Create(userAnswerTemp);

        public void DeleteTempAnswer(UserAnswerTemp userAnswerTemp) => Remove(userAnswerTemp);
        public void UpdateTempAnswer(UserAnswerTemp entity) => Update(entity);


        public UserAnswerTemp? GetOneTempAnswer(int userQuizInfoTempId, int questionId, bool trackChanges)
        {
            return FindByCondition(ua => ua.UserQuizInfoTempId == userQuizInfoTempId && ua.QuestionId == questionId, trackChanges);
        }

        public IEnumerable<UserAnswerTemp> GetTempAnswersByTempInfoId(int userQuizInfoTempId, bool trackChanges)
        {
            return FindAllByCondition(ua => ua.UserQuizInfoTempId == userQuizInfoTempId, trackChanges)
                .ToList();
        }


    }
}
