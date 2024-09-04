using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuestionDtoForInsertion 
    {
        public int Order { get; set; } 
        public string QuestionText { get; set; } = string.Empty; 
        public List<OptionDtoForInsertion> Options { get; set; } = new List<OptionDtoForInsertion>();
        public int CorrectOptionId { get; set; }   
        public int QuizId { get; set; }  
    }
}
