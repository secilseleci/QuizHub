using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string? Title { get; set; }
        public ICollection<Question> Questions { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public int QuestionCount { get; set; }
        public bool ShowCase { get; set; }
        public ICollection<Department> Departments { get; set; }

    }
}
