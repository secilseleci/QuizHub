using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class InfoDto
    {
        public int UserQuizInfoId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Score { get; set; }
        public bool IsSuccessful { get; set; }
        public int CorrectAnswer { get; set; }
        public int FalseAnswer { get; set; }


        public ICollection<UserAnswer> UserAnswers { get; set; }  // UserAnswer ile ilişki
    }
}
