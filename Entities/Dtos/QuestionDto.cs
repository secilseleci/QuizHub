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
        public int Order { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int CorrectOptionId { get; set; }
        public int QuizId { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
    }
}
