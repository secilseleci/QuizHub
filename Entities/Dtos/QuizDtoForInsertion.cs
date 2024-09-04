using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuizDtoForInsertion
    {
        public int QuestionCount { get; set; }

        public string Title { get; set; } = string.Empty;
        public bool ShowCase { get; set; }
        public List<QuestionDtoForInsertion> Questions { get; set; } = new List<QuestionDtoForInsertion>();
    }
}
