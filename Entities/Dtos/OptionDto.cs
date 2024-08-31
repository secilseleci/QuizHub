using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class OptionDto
    {
        public int OptionId { get; set; } 
        public string OptionText { get; set; }  
        public bool IsCorrect { get; set; }  
        public int QuestionId { get; set; }   
    }
}
