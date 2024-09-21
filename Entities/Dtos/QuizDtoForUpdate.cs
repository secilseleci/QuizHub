using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuizDtoForUpdate 
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public int QuestionCount { get; set; }
        public bool ShowCase { get; set; }
        public DateTime UpdatedDate { get; set; }= DateTime.Now;

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
