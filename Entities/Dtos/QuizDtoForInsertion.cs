using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuizDtoForInsertion
    {
        public int QuestionCount { get; set; }
        [Required(ErrorMessage = "Title is required")] 
        public string Title { get; set; } = string.Empty;
        public bool ShowCase { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
