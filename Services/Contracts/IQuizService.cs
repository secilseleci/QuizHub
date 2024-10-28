using Entities.Models;
using Entities.Dtos;
using Entities.RequestParameters;
using Entities.Exeptions;

namespace Services.Contracts
{
    public interface IQuizService
    {
        Result<string> CreateQuiz(QuizDtoForInsertion quizDto);
        void UpdateOneQuiz(QuizDtoForUpdate quizDto);
        void DeleteOneQuiz(int id);
        IEnumerable<Quiz> GetAllQuizzes(bool trackChanges);
        Quiz? GetOneQuiz(int id, bool trackChanges);
        IEnumerable<Quiz> GetShowCaseQuizzes(bool trackChanges);
        IEnumerable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q);
        Quiz? GetQuizWithDetails(int quizId, bool trackChanges);
        QuizDtoForUpdate GetOneQuizForUpdate(int id, bool trackChanges);

        void AssignQuizToDepartments(int quizId, List<int> departmentIds);

        IQueryable<Quiz> GetQuizzesWithDepartments(bool trackChanges);
        IEnumerable<Department> GetDepartmentsByQuizId(int quizId, bool trackChanges);
        Question GetNextQuestion(int quizId, int currentQuestionOrder);
        IEnumerable<Quiz> GetPendingQuizzesForUser(string userId, bool trackChanges);
        QuizDtoForUser ContinueQuiz(int quizId, string userId);
    }
}