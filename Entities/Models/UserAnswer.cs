using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UserAnswer
    {
        public int UserAnswerId { get; set; }           
        [ForeignKey("UserQuizInfo")]
        public int UserQuizInfoId { get; set; }         
        public UserQuizInfo UserQuizInfo { get; set; }  

        public int QuestionId { get; set; }             
        public Question Question { get; set; }          

        public int SelectedOptionId { get; set; }       
        public Option SelectedOption { get; set; }       

        public bool IsCorrect { get; set; }            
    }

}
