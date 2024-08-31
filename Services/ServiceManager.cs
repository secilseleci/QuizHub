using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IOptionService _optionService;
        private readonly IAuthService _authService;
        public ServiceManager(IQuizService quizService,IAuthService authService,IOptionService optionService,IQuestionService questionService)
        {
            _quizService = quizService;
            _authService = authService;
            _questionService = questionService;
            _optionService = optionService;
        }

        public IQuizService QuizService => _quizService;
        public IQuestionService QuestionService => _questionService;
        public IOptionService OptionService => _optionService;
        public IAuthService AuthService => _authService;
    }
}
