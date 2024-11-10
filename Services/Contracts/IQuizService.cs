using Entities.Models;
using Entities.Dtos;
using Entities.Exeptions;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface IQuizService
    {
        Task<ResultGeneric<Quiz>> CreateOneQuiz(QuizDtoForInsertion quizDto);
        Task<ResultGeneric<Quiz>> UpdateOneQuiz(QuizDtoForUpdate quizDto);
        Task<Result> DeleteOneQuiz(int id);
        Task<ResultGeneric<QuizDtoForUpdate>> GetOneQuizForUpdate(int id, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Quiz>>> GetAllQuizzes(bool trackChanges);
        Task<ResultGeneric<Quiz>> GetOneQuiz(int id, bool trackChanges);
        Task<ResultGeneric<Quiz>> GetQuizWithDetails(int quizId, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Quiz>>> GetShowCaseQuizzes(bool trackChanges);
        
        Task<Result> UploadExcel(IFormFile file);
        Task<Result> AssignQuizToDepartments(int quizId, List<int> departmentIds);

        Task<ResultGeneric<IEnumerable<Quiz>>> GetPendingQuizzesForUser(string userId, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Quiz>>> GetQuizzesWithDepartmentsAsync(bool trackChanges);

        Task<ResultGeneric<Quiz>> GetQuizWithDepartments(int quizId, bool trackChanges);
    Task<ResultGeneric<IEnumerable<Quiz>>> GetQuizzesByDepartmentId(int departmentId, bool trackChanges);


        Task<ResultGeneric<QuizDtoForUser>> ContinueQuiz(int quizId, string userId);

        Task<ResultGeneric<QuizDtoForUser>> StartQuiz(int quizId, string userId);
        Task<Result> SaveAnswer(int quizId, int questionId, int selectedOptionId, string userId);
    }
}