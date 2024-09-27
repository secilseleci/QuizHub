using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestParameters;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;



namespace Services
{
    public class QuizManager : IQuizService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly RepositoryContext _context;

        public QuizManager(IRepositoryManager manager, IMapper mapper, RepositoryContext context)
        {
            _manager = manager;
            _mapper = mapper;
            _context = context;
        }

        public void CreateQuiz(QuizDtoForInsertion quizDto)
        {
            var quiz = _mapper.Map<Quiz>(quizDto);

            _manager.Quiz.Create(quiz);
            quiz.QuestionCount = quiz.Questions.Count;
            _manager.Save();

            foreach (var question in quiz.Questions)
            {
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null)
                {
                    question.CorrectOptionId = correctOption.OptionId;
                }
            }

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
        public void UpdateOneQuiz(QuizDtoForUpdate quizDto)
        {
            var existingQuiz = _manager.Quiz.GetQuizWithDetails(quizDto.QuizId, false);  // AsNoTracking ile sorgulayın
            if (existingQuiz == null)
                throw new Exception("Quiz not found!");

            _mapper.Map(quizDto, existingQuiz);
            _manager.Quiz.Update(existingQuiz);

            _manager.Save();
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
        public IEnumerable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q)
        {
            return _manager.Quiz.GetAllQuizzesWithDetails(q);
        }
        public Quiz? GetQuizWithDetails(int quizId, bool trackChanges)
        {
            return _manager.Quiz.GetQuizWithDetails(quizId, trackChanges);

        }
        public QuizDtoForUpdate GetOneQuizForUpdate(int id, bool trackChanges)
        {
            var quiz = _manager.Quiz.GetQuizWithDetails(id, trackChanges);

            if (quiz == null)
                throw new Exception("Quiz not found!");

            var quizDto = _mapper.Map<QuizDtoForUpdate>(quiz);
            return quizDto;
        }
        public void AssignQuizToDepartments(int quizId, List<int> departmentIds)
        {
            var quiz = _manager.Quiz.GetOneQuiz(quizId, true);

            if (quiz == null)
            {
                throw new Exception("Quiz bulunamadı!");
            }
            if (quiz.Departments == null)
            {
                quiz.Departments = new List<Department>();
            }
            foreach (var departmentId in departmentIds)
            {
                var department = _manager.Department.GetOneDepartment(departmentId,true);

                if (department == null)
                {
                    throw new Exception($"Department {departmentId} bulunamadı!");
                }

                if (!quiz.Departments.Contains(department))
                {
                    quiz.Departments.Add(department);
                }
                _manager.Save();  

            }

        }

            public IEnumerable<Quiz> GetShowCaseQuizzes(bool trackChanges)
                {
            var quizzes = _manager.Quiz.GetShowCaseQuizzes(trackChanges);
            return quizzes;
                 }


            public IQueryable<Quiz> GetQuizzesWithDepartments(bool trackChanges)
            {
                return _context.Quizzes
                               .Include(q => q.Departments)  
                               .AsQueryable();
            }
    }
}