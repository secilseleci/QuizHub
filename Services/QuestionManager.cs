using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

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

        public void CreateOneQuestion(QuestionDtoForInsertion questionDto)
        {
            Question question = _mapper.Map<Question>(questionDto);
            _manager.Question.Create(question);
            _manager.Save();
        }

        public void DeleteOneQuestion(int id)
        {
            Question question = GetOneQuestion(id, false);
            if (question is not null)
            {
                _manager.Question.DeleteOneQuestion(question);
                _manager.Save();
            }
        }

        public IEnumerable<Question> GetAllQuestions(bool trackChanges)
        {
            return _manager.Question.GetAllQuestions(trackChanges);
        }

        public Question? GetOneQuestion(int id, bool trackChanges)
        {
            var question= _manager.Question.GetOneQuestion(id, trackChanges);
            if (question is null)
                throw new Exception("question not found!");
            return question;
        }

       
        public void UpdateOneQuestion(QuestionDtoForUpdate questionDto)
        {
            if (questionDto == null)
            {
                throw new ArgumentNullException(nameof(questionDto), "Question data cannot be null.");
            }

            var entity = _mapper.Map<Question>(questionDto);
            if (entity == null)
            {
                throw new Exception("Mapping failed. Question entity is null.");
            }

            _manager.Question.UpdateOneQuestion(entity);
            _manager.Save();
        }

        public Question? GetOneQuestionWithOptions(int id, bool trackChanges)
        {
            var question = _manager.Question.GetOneQuestionWithOptions(id, trackChanges);
            if (question is null)
                throw new Exception("Question not found!");
            return question;
        }

        public IEnumerable<Question> GetQuestionsByQuizId(int quizId, bool trackChanges)
        {
            return _manager.Question.GetQuestionsByQuizId(quizId, trackChanges);

        }
    }
    }

