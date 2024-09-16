﻿using Microsoft.AspNetCore.Identity;  // IdentityUser için gerekli

namespace Entities.Models
{
    public class UserQuizInfo
        {
            public int UserQuizInfoId { get; set; }

            public string UserId { get; set; }  // IdentityUser'daki Id, string tipindedir
            public IdentityUser User { get; set; }  // IdentityUser ile ilişkilendirme

            public int QuizId { get; set; }
            public Quiz Quiz { get; set; }

            public bool IsCompleted { get; set; } 
            public DateTime? CompletedAt { get; set; }
            public int Score { get; set; }
            public bool IsSuccessful { get; set; }
            public int CorrectAnswer { get; set; }
            public int FalseAnswer { get; set; }
            public int BlankAnswer { get; set; }
           
        
        public ICollection<UserAnswer> UserAnswers { get; set; }  // UserAnswer ile ilişki
            
        }

}