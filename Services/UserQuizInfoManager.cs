using AutoMapper;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserQuizInfoManager : IUserQuizInfoService
    {
        private readonly IRepositoryManager _manager; // RepositoryManager ile çalışıyoruz
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
    }
}
