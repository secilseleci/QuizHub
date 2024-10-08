﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq;

namespace Repositories
{
    public class OptionRepository : RepositoryBase<Option>, IOptionRepository
    {
        public OptionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateOneOption(Option option) => Create(option);
        public void UpdateOneOption(Option entity) => Update(entity);
        public void DeleteOneOption(Option option) => Remove(option);


        public IQueryable<Option> GetAllOptions(bool trackChanges) => FindAll(trackChanges);


        public Option? GetOneOption(int id, bool trackChanges)
        {
            return FindByCondition(o => o.OptionId.Equals(id), trackChanges);
        }

        public IQueryable<Option> GetOptionsByQuestionId(int questionId, bool trackChanges)
        {
            return FindAllByCondition(o => o.QuestionId == questionId, trackChanges);
        }

        public Option? GetCorrectOptionForQuestion(int questionId, bool trackChanges)
        {
            return FindByCondition(o => o.QuestionId == questionId && o.IsCorrect, trackChanges);
        }
    }
}
