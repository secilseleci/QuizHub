using Entities.Models;
using Repositories.Contracts;

namespace Repositories.Implementations
{
    public class OptionRepository : RepositoryBase<Option>, IOptionRepository
    {
        public OptionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateOneOptionAsync(Option option) => await CreateAsync(option);
        public async Task UpdateOneOptionAsync(Option entity) => await UpdateAsync(entity);
        public async Task DeleteOneOptionAsync(Option option) => await RemoveAsync(option);


        public async Task<IQueryable<Option>> GetAllOptionsAsync(bool trackChanges) 
            => await FindAllAsync(trackChanges);

        public async Task<IQueryable<Option>> GetOptionsByQuestionIdAsync(int questionId, bool trackChanges)
           => await FindAllByConditionAsync(o => o.QuestionId == questionId, trackChanges);
         
        public async Task<Option?> GetOneOptionAsync(int id, bool trackChanges)
           => await FindByConditionAsync(o => o.OptionId.Equals(id), trackChanges);
      
        public async Task<Option?> GetCorrectOptionForQuestionAsync(int questionId, bool trackChanges)
           => await FindByConditionAsync(o => o.QuestionId == questionId && o.IsCorrect, trackChanges);
        
    }
}
