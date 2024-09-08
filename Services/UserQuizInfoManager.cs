using AutoMapper;
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

    }
}