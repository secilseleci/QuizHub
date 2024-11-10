using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IOptionService
    {
        Task<ResultGeneric<Option>> CreateOneOption(OptionDto optionDto);
        Task<ResultGeneric<Option>> UpdateOneOption(OptionDto optionDto);
        Task<Result> DeleteOneOption(int id);
        Task<ResultGeneric<IEnumerable<Option>>> GetAllOptions(bool trackChanges);
        Task<ResultGeneric<Option>> GetOneOption(int id, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Option>>> GetOptionsByQuestionId(int questionId, bool trackChanges);
        Task<ResultGeneric<Option>> GetCorrectOptionForQuestion(int questionId, bool trackChanges);
    }
}
