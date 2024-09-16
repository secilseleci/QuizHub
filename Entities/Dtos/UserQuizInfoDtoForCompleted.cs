using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserQuizInfoDtoForCompleted
    {
        public int CorrectAnswer { get; set; }
        public int FalseAnswer { get; set; }
        public int BlankAnswer { get; set; }
        public int Score { get; set; }
        public bool IsSuccessfull {  get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
