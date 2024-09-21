using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using System.Linq.Expressions;


namespace Repositories { 
public class UserQuizInfoRepository : RepositoryBase<UserQuizInfo>, IUserQuizInfoRepository
{

    public UserQuizInfoRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {

    }

    public void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo) => Create(userQuizInfo);
    public void UpdateOneUserQuizInfo(UserQuizInfo entity) => Update(entity);
    public void RemoveOneUserQuizInfo(UserQuizInfo userQuizInfo) => Remove(userQuizInfo);
    // ID'si eşleşen tek kaydı getir
    public UserQuizInfo GetUserQuizInfoById(int userQuizInfoId, bool trackChanges)
        {
            return FindByCondition(uqi => uqi.UserQuizInfoId == userQuizInfoId, trackChanges);  
        }
    // Kullanıcının belirli bir quize ait bilgilerini getirir
    public UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            return trackChanges
                ? _context.UserQuizInfo.SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId)
                : _context.UserQuizInfo.AsNoTracking().SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId);
        }


     //Kullanıcının tüm quiz sonuçlarını getir
     public IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges)
        {
            return trackChanges
                ? _context.UserQuizInfo.Where(uqi => uqi.UserId == userId).ToList()
                : _context.UserQuizInfo.AsNoTracking().Where(uqi => uqi.UserId == userId).ToList();
        }

    }
}