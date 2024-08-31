using AutoMapper;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class QuestionManager : IQuestionService
    {

        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public QuestionManager(IRepositoryManager manager,
        IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }
        public IEnumerable<Question> GetAllQuestions(bool trackChanges)
        {
            return _manager.Question.GetAllQuestions(trackChanges);
        }

        public Question? GetOneQuestion(int id, bool trackChanges)
        {
            var product = _manager.Question.GetOneQuestion(id, trackChanges);
            if (product is null)
                throw new Exception("Question not found!");
            return product;
        }
    }
}
