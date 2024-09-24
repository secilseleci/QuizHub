using AutoMapper;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserQuizInfoManager : IUserQuizInfoService
    {
        private readonly IRepositoryManager _manager; 
        private readonly IMapper _mapper;

        public UserQuizInfoManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            return _manager.UserQuizInfo.GetUserQuizInfoByQuizIdAndUserId(quizId, userId, trackChanges);
        }

        public IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges)
        {
          return _manager.UserQuizInfo.GetUserQuizInfoByUserId(userId, trackChanges);
         
        }

        public void AssignQuizToUsers(int quizId, List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                // Aynı quiz zaten atanmışsa tekrar ekleme
                var existingAssignment = _manager.UserQuizInfo
                    .GetUserQuizInfoByQuizIdAndUserId(quizId, userId, false);

                if (existingAssignment == null)
                {
                    var assignment = new UserQuizInfo
                    {
                        QuizId = quizId,
                        UserId = userId,
                        IsCompleted = false,
                        Score = 0,
                        CorrectAnswer = 0,
                        FalseAnswer = 0,
                        BlankAnswer = 0
                    };

                    _manager.UserQuizInfo.CreateOneUserQuizInfo(assignment);
                }
            }

            _manager.Save();
        }

        public void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            _manager.UserQuizInfo.CreateOneUserQuizInfo(userQuizInfo);
        }
    }
}