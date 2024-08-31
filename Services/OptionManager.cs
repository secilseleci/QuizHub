using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OptionManager : IOptionService
    {
        public IEnumerable<Option> GetAllOptions(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Option? GetOneOption(int id, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
