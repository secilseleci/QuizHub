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
        public int Score { get; set; }

    }
}
