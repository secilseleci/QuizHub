using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Repositories.Contracts;
using Services.Contracts;



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
            if (quizDto == null)
            {
                throw new ArgumentNullException(nameof(quizDto), "Quiz data cannot be null.");
            }

            var existingQuiz = _manager.Quiz.GetOneQuiz(quizDto.QuizId, true);

            if (existingQuiz == null)
            {
                throw new Exception("Quiz not found!");
            }

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