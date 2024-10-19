using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class UserQuizInfoTemp
    {
        public int UserQuizInfoTempId { get; set; }  

        public string UserId { get; set; }          
        public ApplicationUser User { get; set; }    

        public int QuizId { get; set; }               
        public Quiz Quiz { get; set; }               

        public bool IsCompleted { get; set; }         
        public DateTime? StartedAt { get; set; }      

        public int CorrectAnswer { get; set; }      
        public int FalseAnswer { get; set; }        
        public int Score { get; set; }    

        public ICollection<UserAnswerTemp> UserAnswersTemp { get; set; }  
    }
}
