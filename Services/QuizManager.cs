﻿using AutoMapper;
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
        public void AssignQuizToDepartments(int quizId, List<int> selectedDepartmentIds)
        {
            // 1. Quiz'i departmanları ile birlikte alıyoruz
            var quiz = _manager.Quiz.GetQuizWithDepartments(quizId, true);
            if (quiz == null)
            {
                throw new Exception("Quiz bulunamadı!");
            }

            // 2. Eğer quiz.Departments null ise, onu başlatıyoruz
            if (quiz.Departments == null)
            {
                quiz.Departments = new List<Department>(); // Null ise yeni bir liste oluşturuyoruz
            }

            // 3. Mevcut atanmış departman ID'lerini alıyoruz
            var currentDepartmentIds = quiz.Departments.Select(d => d.DepartmentId).ToList();

            // 4. Kaldırılması gereken departmanlar (artık seçilmemiş olanlar)
            var departmentsToRemove = currentDepartmentIds
                .Where(id => !selectedDepartmentIds.Contains(id)) // Formda işaretlenmeyen departmanlar
                .ToList();

            // 5. Eklenmesi gereken departmanlar (yeni seçilenler)
            var departmentsToAdd = selectedDepartmentIds
                .Where(id => !currentDepartmentIds.Contains(id)) // Daha önce atanmış olmayanlar
                .ToList();

            // 6. Kaldırılacak departmanları quiz'den siliyoruz
            foreach (var departmentId in departmentsToRemove)
            {
                var departmentToRemove = quiz.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);
                if (departmentToRemove != null)
                {
                    quiz.Departments.Remove(departmentToRemove); // Departmanı ilişkiden kaldırıyoruz
                }
            }

            // 7. Yeni eklenmesi gereken departmanları quiz'e ekliyoruz
            foreach (var departmentId in departmentsToAdd)
            {
                var departmentToAdd = _manager.Department.GetOneDepartment(departmentId, true);
                if (departmentToAdd == null)
                {
                    throw new Exception($"Department {departmentId} bulunamadı!");
                }

                quiz.Departments.Add(departmentToAdd); // Yeni departmanı ekliyoruz
            }

            // 8. Değişiklikleri kaydediyoruz
            _manager.Save();
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
        public IEnumerable<Department> GetDepartmentsByQuizId(int quizId, bool trackChanges)
        {
            var quiz = _manager.Quiz.GetQuizWithDepartments(quizId, trackChanges);

            if (quiz == null)
            {
                throw new Exception("Quiz bulunamadı!");
            }

            return quiz.Departments;
        }

        public Question GetNextQuestion(int quizId, int currentQuestionOrder)
        {
            return _manager.Question
       .GetQuestionsByQuizId(quizId, trackChanges: false)
       .Include(q => q.Options)  
       .Where(q => q.Order > currentQuestionOrder)
       .OrderBy(q => q.Order)
       .FirstOrDefault();
        }

        public IEnumerable<Quiz> GetPendingQuizzesForUser(string userId, bool trackChanges)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return new List<Quiz>();

            var departmentId = user.DepartmentId;

            var allQuizzes = _manager.Quiz.GetQuizzesByDepartmentId(departmentId, trackChanges);

            var solvedQuizIds = _manager.UserQuizInfo.GetUserQuizInfoByUserId(userId, trackChanges)
                                                       .Select(uqi => uqi.QuizId)
                                                       .ToList();

            var inProgressQuizIds = _manager.UserQuizInfoTemp.GetIncompleteQuizzesByUserId(userId, trackChanges)
                                                               .Select(uqi => uqi.QuizId)
                                                               .ToList();

            var pendingQuizzes = allQuizzes
                .Where(q => !solvedQuizIds.Contains(q.QuizId) && !inProgressQuizIds.Contains(q.QuizId))
                .ToList();

            return pendingQuizzes;
        }
        public QuizDtoForUser ContinueQuiz(int quizId, string userId)
        {
            var userQuizInfoTemp = _manager.UserQuizInfoTemp.GetTempInfoByQuizIdAndUserId(quizId, userId, trackChanges: false);
            if (userQuizInfoTemp == null)
            {
                throw new Exception("Quiz bilgisi bulunamadı.");
            }
            var lastUserAnswerTemp = _manager.UserAnswerTemp
            .GetTempAnswersByTempInfoId(userQuizInfoTemp.UserQuizInfoTempId, trackChanges: false)
            .OrderByDescending(a => a.QuestionId) // Order'a göre soruları sıraladık
            .FirstOrDefault(); // Eğer varsa en son cevabı bulduk

            int currentQuestionOrder;
            if (lastUserAnswerTemp == null)
            {
                // Eğer hiç cevap yoksa, quiz en baştan başlasın
                currentQuestionOrder = 0;
            }
            else
            {
                // Cevap varsa, en son cevaplanan sorunun sırası ile başlayalım
                currentQuestionOrder = _manager.Question.GetOneQuestion(lastUserAnswerTemp.QuestionId, trackChanges: false).Order;
            }
            var quiz = _manager.Quiz.GetQuizWithDetails(quizId, trackChanges: false);
            if (quiz == null)
            {
                throw new Exception("Quiz bulunamadı.");
            }
            var nextQuestion = quiz.Questions
          .Where(q => q.Order > currentQuestionOrder) // En son cevaplanan sorunun sonrasını getir
          .OrderBy(q => q.Order)
          .FirstOrDefault();
            if (nextQuestion == null)
            {
                throw new Exception("Sıradaki soru bulunamadı.");
            }

            var quizDto = _mapper.Map<QuizDtoForUser>(quiz);
            quizDto.QuestionCount = quiz.Questions.Count;
            quizDto.Questions = new List<Question> { nextQuestion };  // Sadece sıradaki soruyu gönderiyoruz

            return quizDto;
        }
    }
}