using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class UserAnswerTemp
    {
        public int UserAnswerTempId { get; set; }           

        [ForeignKey("UserQuizInfoTemp")]
        public int UserQuizInfoTempId { get; set; }        
        public UserQuizInfoTemp UserQuizInfoTemp { get; set; }   

        public int QuestionId { get; set; }                 
        public Question Question { get; set; }              
 
        public int SelectedOptionId { get; set; }           
        public Option SelectedOption { get; set; }          

        public bool IsCorrect { get; set; }                 
    }
}
