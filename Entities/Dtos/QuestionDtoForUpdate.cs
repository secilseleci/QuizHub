using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuestionDtoForUpdate
    {
        public int Order { get; set; }

        public int QuestionId { get; set; }  
        public string QuestionText { get; set; }  
        public int CorrectOptionId { get; set; }  
        public List<OptionDtoForUpdate> Options { get; set; }  = new List<OptionDtoForUpdate>();
    }
}
