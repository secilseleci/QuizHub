namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IQuizRepository Quiz { get; }
        IQuestionRepository Question { get; }
        IOptionRepository Option { get; }
        IUserQuizInfoRepository UserQuizInfo { get; }
        IUserAnswerRepository UserAnswer { get; }
        IDepartmentRepository Department { get; }  
        IUserQuizInfoTempRepository UserQuizInfoTemp { get; }
        IUserAnswerTempRepository UserAnswerTemp { get; }
        Task<bool> SaveAsync();
    }
}
