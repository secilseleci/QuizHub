using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using System.Linq.Expressions;

public class UserQuizInfoRepository : RepositoryBase<UserQuizInfo>, IUserQuizInfoRepository
{
 
    public UserQuizInfoRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
        
    }
     
    public void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo) => Create(userQuizInfo);
    public void UpdateOneUserQuizInfo(UserQuizInfo entity) => Update(entity);
    public void RemoveOneUserQuizInfo(UserQuizInfo userQuizInfo) => Remove(userQuizInfo);
   

    public UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
    {
        return trackChanges
            ? _context.UserQuizInfo.SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId)
            : _context.UserQuizInfo.AsNoTracking().SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId);
    }
    public UserQuizInfo GetUserQuizInfoById(int userQuizInfoId, bool trackChanges)
    {
        return FindByCondition(uqi => uqi.UserQuizInfoId == userQuizInfoId, trackChanges);  // ID'si eşleşen tek kaydı getir
    }


}
