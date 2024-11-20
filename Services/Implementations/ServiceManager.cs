using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IOptionService _optionService;
        private readonly IAuthService _authService;
        private readonly IUserQuizInfoService _userQuizInfoService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserQuizInfoTempService _userQuizInfoTempService;
        private readonly IUserAnswerTempService _userAnswerTempService;
        private readonly IUserProfileService _userProfileService;
        public ServiceManager(
            IQuizService quizService,
            IAuthService authService,
            IOptionService optionService,
            IUserQuizInfoService userQuizInfoService,
            IUserQuizInfoTempService userQuizInfoTempService,
            IQuestionService questionService,
            IUserAnswerService userAnswerService,
            IUserAnswerTempService userAnswerTempService,
            IDepartmentService departmentService,
            IUserProfileService userProfileService)
        {
            _quizService = quizService;
            _authService = authService;
            _questionService = questionService;
            _optionService = optionService;
            _userQuizInfoService = userQuizInfoService;
            _userQuizInfoTempService = userQuizInfoTempService;
            _userAnswerService = userAnswerService;
            _userAnswerTempService = userAnswerTempService;
            _departmentService = departmentService;
            _userProfileService = userProfileService;
        }

        public IQuizService QuizService => _quizService;
        public IQuestionService QuestionService => _questionService;
        public IOptionService OptionService => _optionService;
        public IAuthService AuthService => _authService;
        public IUserAnswerService UserAnswerService => _userAnswerService;
        public IUserQuizInfoService UserQuizInfoService => _userQuizInfoService;
        public IDepartmentService DepartmentService => _departmentService;
        public IUserQuizInfoTempService UserQuizInfoTempService => _userQuizInfoTempService;
        public IUserAnswerTempService UserAnswerTempService => _userAnswerTempService;
        public IUserProfileService UserProfileService => _userProfileService;   
    }
}
