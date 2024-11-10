using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;

namespace Services.Contracts
{
    public interface IQuestionService
    {
        Task<ResultGeneric<Question>> CreateOneQuestion(QuestionDto questionDto);
        Task<ResultGeneric<Question>> UpdateOneQuestion(QuestionDto questionDto);
        Task<Result> DeleteOneQuestion(int id);
        Task<ResultGeneric<IEnumerable<Question>>> GetAllQuestions(bool trackChanges);
        Task<ResultGeneric<Question>> GetOneQuestion(int id, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Question>>> GetQuestionsByQuizId(int quizId, bool trackChanges);
        Task<ResultGeneric<Question>> GetOneQuestionWithOptions(int id, bool trackChanges);
        Task<ResultGeneric<QuestionDto>> GetNextQuestion(int quizId, int currentQuestionOrder,int selectedOptionId);

    }
}
