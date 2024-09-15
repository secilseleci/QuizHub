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
        public int UserAnswerId { get; set; }          // UserAnswer için benzersiz ID
        [ForeignKey("UserQuizInfo")]
        public int UserQuizInfoId { get; set; }         // Hangi UserQuizInfo'ya ait olduğunu belirtir
        public UserQuizInfo UserQuizInfo { get; set; }  // UserQuizInfo ile ilişki

        public int QuestionId { get; set; }             // Hangi soruya ait olduğunu belirtir
        public Question Question { get; set; }          // Question ile ilişki

        public int? SelectedOptionId { get; set; }       // Kullanıcının seçtiği şıkkın ID'si
        public Option SelectedOption { get; set; }      // Seçilen şık ile ilişki

        public bool IsCorrect { get; set; }             // Cevap doğru mu, yanlış mı
    }

}
