using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Dtos;
using Entities.RequestParameters;



namespace Services
{
    public class QuizManager : IQuizService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public QuizManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }



        public IEnumerable<Quiz> GetAllQuizzes(bool trackChanges)
        {
            return _manager.Quiz.GetAllQuizzes(trackChanges);

        }


        public Quiz? GetOneQuiz(int id, bool trackChanges)
        {
            var quiz = _manager.Quiz.GetOneQuiz(id, trackChanges);
            if (quiz is null)
                throw new Exception("Quiz not found!");
            return quiz;
        }


        public void UpdateOneQuiz(QuizDtoForUpdate quizDto)
        {
            var entity = _mapper.Map<Quiz>(quizDto);
            _manager.Quiz.UpdateOneQuiz(entity);
            _manager.Save();
        }


        public QuizDtoForUpdate GetOneQuizForUpdate(int id, bool trackChanges)
        {
            var quiz = GetOneQuiz(id, trackChanges);
            var quizDto = _mapper.Map<QuizDtoForUpdate>(quiz);
            return quizDto;
        }


        public void CreateQuiz(QuizDtoForInsertion quizDto)
        {
            Quiz quiz = _mapper.Map<Quiz>(quizDto);
            _manager.Quiz.Create(quiz);
            _manager.Save();
        }


        public void DeleteOneQuiz(int id)
        {
            Quiz quiz = GetOneQuiz(id, false);
            if (quiz is not null)
            {
                _manager.Quiz.DeleteOneQuiz(quiz);
                _manager.Save();
            }
        }
        public IEnumerable<Quiz> GetShowCaseQuizzes(bool trackChanges)
        {
            var quizzes = _manager.Quiz.GetShowCaseQuizzes(trackChanges);
            return quizzes;
        }

        public IEnumerable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q)
        {
            return _manager.Quiz.GetAllQuizzesWithDetails(q);
        }
    }
}