using Repositories.Contracts;

namespace Repositories.Implementations
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IUserQuizInfoRepository _userQuizInfoRepository;
        private readonly IUserAnswerRepository _userAnswerRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserQuizInfoTempRepository _userQuizInfoTempRepository;
        private readonly IUserAnswerTempRepository _userAnswerTempRepository;
        public RepositoryManager(RepositoryContext context,
                                 IQuizRepository quizRepository,
                                 IQuestionRepository questionRepository,
                                 IOptionRepository optionRepository,
                                 IUserQuizInfoRepository userQuizInfoRepository,
                                 IUserAnswerRepository userAnswerRepository,
                                 IDepartmentRepository departmentRepository,
                                 IUserQuizInfoTempRepository userQuizInfoTempRepository,
                                 IUserAnswerTempRepository userAnswerTempRepository
                                 )
        {
            _context = context;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _userQuizInfoRepository = userQuizInfoRepository;
            _userAnswerRepository = userAnswerRepository;
            _departmentRepository = departmentRepository;
            _userAnswerTempRepository = userAnswerTempRepository;
            _userQuizInfoTempRepository = userQuizInfoTempRepository;
        }

        public IQuizRepository Quiz => _quizRepository;
        public IQuestionRepository Question => _questionRepository;
        public IOptionRepository Option => _optionRepository;
        public IUserQuizInfoRepository UserQuizInfo => _userQuizInfoRepository;
        public IUserAnswerRepository UserAnswer => _userAnswerRepository;
        public IDepartmentRepository Department => _departmentRepository;  
        public IUserQuizInfoTempRepository UserQuizInfoTemp => _userQuizInfoTempRepository;
        public IUserAnswerTempRepository UserAnswerTemp => _userAnswerTempRepository;
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
