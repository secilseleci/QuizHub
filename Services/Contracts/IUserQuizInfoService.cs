using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
      
public interface IUserQuizInfoService
{
    UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);
    IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges);
    void AssignQuizToUsers(int quizId, List<string> userIds);
    void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo);
    void UpdateOneUserQuizInfo(UserQuizInfo userQuizInfo);
    UserQuizInfo GetUserQuizInfoById(int userQuizInfoId, bool trackChanges);
    UserQuizInfo ProcessQuiz(int quizId, string userId);
    UserQuizInfo SaveQuiz(UserQuizInfoTemp userQuizInfoTemp, IEnumerable<UserAnswerTemp> userAnswersTemp);
    UserQuizInfo UpdateQuiz(UserQuizInfo existingQuizInfo, IEnumerable<UserAnswerTemp> userAnswersTemp, UserQuizInfoTemp userQuizInfoTemp);

    IEnumerable<UserQuizInfo> GetRetakeableQuizzesByUserId(string userId, bool trackChanges);
    IEnumerable<UserQuizInfo> GetCompletedQuizzesByUserId(string userId, bool trackChanges);

    }
}


