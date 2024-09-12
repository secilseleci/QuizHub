using Entities.Models;
using Entities.Dtos;
using Entities.RequestParameters;

namespace Services.Contracts
{
    public interface IQuizService
    {
        IEnumerable<Quiz> GetAllQuizzes(bool trackChanges);

        IEnumerable<Quiz> GetShowCaseQuizzes(bool trackChanges);
        IEnumerable<Quiz> GetAllQuizzesWithDetails(QuizRequestParameters q);

        Quiz? GetOneQuiz(int id, bool trackChanges);

        void CreateQuiz(QuizDtoForInsertion quizDto);
        void UpdateOneQuiz(QuizDtoForUpdate quizDto);
        void DeleteOneQuiz(int id);

        Quiz? GetQuizWithDetails(int quizId, bool trackChanges);
        QuizDtoForUpdate GetOneQuizForUpdate(int id, bool trackChanges);


        //void AddQuizFromJson(string jsonData);

    }
}