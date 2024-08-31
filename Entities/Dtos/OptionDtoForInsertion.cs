using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class OptionDtoForInsertion
    {
        public string OptionText { get; set; }  
        public bool IsCorrect { get; set; }  
        public int QuestionId { get; set; }  
    }
}
