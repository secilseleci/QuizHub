using Entities.Models;
using System.Linq;

namespace Repositories.Contracts
{
    public interface IOptionRepository : IRepositoryBase<Option>
    {
        IQueryable<Option> GetAllOptions(bool trackChanges);
        IQueryable<Option> GetOptionsByQuestionId(int questionId, bool trackChanges);

        Option? GetOneOption(int id, bool trackChanges);

        Option? GetCorrectOptionForQuestion(int questionId, bool trackChanges);
        void CreateOneOption(Option option);
        void UpdateOneOption(Option entity);
        void DeleteOneOption(Option option);
    }
}
