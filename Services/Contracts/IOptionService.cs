using Entities.Dtos;
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
        IEnumerable<Option> GetAllOptions(bool trackChanges);


        IEnumerable<Option> GetOptionsByQuestionId(int questionId, bool trackChanges);

        Option? GetOneOption(int id, bool trackChanges);

        Option? GetCorrectOptionForQuestion(int questionId, bool trackChanges);
        void CreateOneOption(OptionDto optionDto);
        void UpdateOneOption(OptionDto optionDto);
        void DeleteOneOption(int id);


    }
}
