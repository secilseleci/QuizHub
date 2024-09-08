using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
   
        public class RepositoryManager : IRepositoryManager
        {
            private readonly RepositoryContext _context;
            private readonly IQuizRepository _quizRepository;
            private readonly IQuestionRepository _questionRepository;
            private readonly IOptionRepository _optionRepository;
            private readonly IUserQuizInfoRepository _userQuizInfoRepository;

        public RepositoryManager(
               RepositoryContext context,
               IQuizRepository quizRepository,
               IQuestionRepository questionRepository,
               IOptionRepository optionRepository, 
               IUserQuizInfoRepository userQuizInfoRepository
               )

            {
                _context = context;
                _quizRepository = quizRepository;
                _questionRepository = questionRepository;
                _optionRepository = optionRepository;
                _userQuizInfoRepository = userQuizInfoRepository;
             }

            public IQuizRepository Quiz => _quizRepository;

            public IQuestionRepository Question => _questionRepository;

            public IOptionRepository Option => _optionRepository;
            public IUserQuizInfoRepository UserQuizInfo => _userQuizInfoRepository;


             public void Save()
                {
                    _context.SaveChanges();
                }
        }
    }

