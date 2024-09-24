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

        public ServiceManager(
            IQuizService quizService,
            IAuthService authService,
            IOptionService optionService,
            IUserQuizInfoService userQuizInfoService,
            IQuestionService questionService,
            IUserAnswerService userAnswerService,
            IDepartmentService departmentService)
        {
            _quizService = quizService;
            _authService = authService;
            _questionService = questionService;
            _optionService = optionService;
            _userQuizInfoService = userQuizInfoService;
            _userAnswerService = userAnswerService;
            _departmentService = departmentService;
        }

        public IQuizService QuizService => _quizService;
        public IQuestionService QuestionService => _questionService;
        public IOptionService OptionService => _optionService;
        public IAuthService AuthService => _authService;
        public IUserAnswerService UserAnswerService => _userAnswerService;
        public IUserQuizInfoService UserQuizInfoService => _userQuizInfoService;
        public IDepartmentService DepartmentService => _departmentService;
    }
}
