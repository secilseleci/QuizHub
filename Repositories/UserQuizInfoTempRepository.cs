using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class UserQuizInfoTempRepository : RepositoryBase<UserQuizInfoTemp>, IUserQuizInfoTempRepository
    {
        public UserQuizInfoTempRepository(RepositoryContext context) : base(context)

        {

        }

        public void CreateTempInfo(UserQuizInfoTemp userQuizInfoTemp)
        {
            Create(userQuizInfoTemp);
        }

        public UserQuizInfoTemp GetOneTempInfoByUserId(string userId, bool trackChanges)
        {
            return FindAllByCondition(uqit => uqit.UserId == userId, trackChanges)
                                           .FirstOrDefault();

        }

        public UserQuizInfoTemp GetTempInfoById(int userQuizInfoTempId, bool trackChanges)
        {
            return FindAllByCondition(uqit => uqit.UserQuizInfoTempId == userQuizInfoTempId, trackChanges)
                           .Include(uqit => uqit.UserAnswersTemp)  // Include ile ilişkili UserAnswersTemp kayıtlarını getir
                           .FirstOrDefault();
        }

        public UserQuizInfoTemp? GetTempInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            return FindAllByCondition(uqit => uqit.QuizId == quizId && uqit.UserId == userId, trackChanges)
                            .Include(uqit => uqit.UserAnswersTemp)  // Include ile ilişkili UserAnswersTemp kayıtlarını getir
                            .FirstOrDefault();
        }

        public IQueryable<UserQuizInfoTemp> GetTempInfoByUserId(string userId, bool trackChanges)
        {
            return FindAllByCondition(uqit => uqit.UserId == userId, trackChanges)
                           .Include(uqit => uqit.UserAnswersTemp);
        }
        public IQueryable<UserQuizInfoTemp> GetIncompleteQuizzesByUserId(string userId, bool trackChanges)
        {
            return FindAllByCondition(uq => uq.UserId == userId && !uq.IsCompleted, trackChanges)
                .Include(uq => uq.Quiz);
        }
        public void RemoveTempInfo(UserQuizInfoTemp userQuizInfoTemp)
        {
            Remove(userQuizInfoTemp); // RepositoryBase içindeki Remove metodu kullanılıyor
        }

        public void UpdateTempInfo(UserQuizInfoTemp entity)
        {
            Update(entity); // RepositoryBase içindeki Update metodu kullanılıyor
        }
    }
}
