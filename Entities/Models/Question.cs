using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int Order { get; set; }
        public string QuestionText { get; set; }
        public int CorrectOptionId { get; set; }

        [ForeignKey("QuizId")] 
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }  
        public ICollection<Option> Options { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }  // UserAnswer ile ilişki
        public ICollection<UserAnswerTemp> UserAnswersTemp { get; set; }  // UserAnswer ile ilişki


    }
}
