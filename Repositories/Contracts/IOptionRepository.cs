using Entities.Models;
using System.Linq;

namespace Repositories.Contracts
{
    public interface IOptionRepository : IRepositoryBase<Option>
    { 
        Task CreateOneOptionAsync(Option option);
        Task UpdateOneOptionAsync(Option entity);
        Task DeleteOneOptionAsync(Option option);
        Task<IQueryable<Option>> GetAllOptionsAsync(bool trackChanges);
        Task<IQueryable<Option>> GetOptionsByQuestionIdAsync(int questionId, bool trackChanges);

        Task< Option?> GetOneOptionAsync(int id, bool trackChanges);
        Task<Option?> GetCorrectOptionForQuestionAsync(int questionId, bool trackChanges);
       
    }
}
