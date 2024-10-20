﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserQuizCardDto
    {   public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int QuestionCount { get; set; }
        public int Score { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsSuccesful { get; set; }  
        public string Status { get; set; }  
        public bool CanRetake { get; set; }  
    }

}
