using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuizListDto
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public int QuestionCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
