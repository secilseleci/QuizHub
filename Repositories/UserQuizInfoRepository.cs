using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq.Expressions; 
namespace Repositories
{
    public class UserQuizInfoRepository : RepositoryBase<UserQuizInfo>, IUserQuizInfoRepository
    {
        public UserQuizInfoRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        // Kullanıcının belirli bir quize ait bilgilerini getir
        public UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            return trackChanges
                ? _context.UserQuizInfo.SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId)
                : _context.UserQuizInfo.AsNoTracking().SingleOrDefault(uqi => uqi.QuizId == quizId && uqi.UserId == userId);
        }

        // Kullanıcının tüm quiz sonuçlarını getir
        public IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges)
        {
            return trackChanges
                ? _context.UserQuizInfo.Where(uqi => uqi.UserId == userId).ToList()
                : _context.UserQuizInfo.AsNoTracking().Include(uqi => uqi.Quiz).Where(uqi => uqi.UserId == userId).ToList();
        }

        // Yeni bir UserQuizInfo kaydı ekle
        public void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            Create(userQuizInfo);
        }

        // Var olan bir UserQuizInfo kaydını güncelle
        public void UpdateOneUserQuizInfo(UserQuizInfo entity)
        {
            Update(entity);
        }

        // Var olan bir UserQuizInfo kaydını sil
        public void RemoveOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            Remove(userQuizInfo);
        }

        // ID'si eşleşen tek UserQuizInfo kaydını getir
        public UserQuizInfo GetUserQuizInfoById(int userQuizInfoId, bool trackChanges)
        {
            return FindByCondition(uqi => uqi.UserQuizInfoId == userQuizInfoId, trackChanges);
        }

        public IEnumerable<UserQuizInfo> GetRetakeableQuizzesByUserId(string userId, bool trackChanges)
        {
            return FindAllByCondition(uqi => uqi.UserId == userId
                                          && uqi.IsCompleted
                                          && uqi.Score < 100, trackChanges)
                            .Include(uqi => uqi.Quiz);

        }

        public IEnumerable<UserQuizInfo> GetCompletedQuizzesByUserId(string userId, bool trackChanges)
        {
            return FindAllByCondition(uqi => uqi.UserId == userId
                                                     && uqi.IsCompleted
                                                     && uqi.Score >= 60, trackChanges)
                                       .Include(uqi => uqi.Quiz);
        }
    }
}
