using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int CorrectOptionId { get; set; }
        public int QuizId { get; set; }
        public ICollection<OptionDto> Options { get; set; }
     }
}
