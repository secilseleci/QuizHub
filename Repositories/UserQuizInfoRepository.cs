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
    public IQueryable<UserQuizInfo> FindAll(bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public UserQuizInfo? FindByCondition(Expression<Func<UserQuizInfo, bool>> expression, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    // Kullanıcı ve quizId'ye göre UserQuizInfo'yu bul
    public UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
    {
        return trackChanges
            ? _context.UserQuizInfo.SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId)
            : _context.UserQuizInfo.AsNoTracking().SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId);
    }

  
}
