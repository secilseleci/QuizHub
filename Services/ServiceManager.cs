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

       

        public ServiceManager(IQuizService quizService,IAuthService authService,IOptionService optionService,IUserQuizInfoService userQuizInfoService,IQuestionService questionService)
        {
            _quizService = quizService;
            _authService = authService;
            _questionService = questionService;
            _optionService = optionService;
            _userQuizInfoService = userQuizInfoService;
        }

        public IQuizService QuizService => _quizService;
        public IQuestionService QuestionService => _questionService;
        public IOptionService OptionService => _optionService;
        public IAuthService AuthService => _authService;

        public IUserQuizInfoService UserQuizInfoService => _userQuizInfoService;
    }
}
