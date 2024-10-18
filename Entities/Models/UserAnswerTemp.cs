using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class UserAnswerTemp
    {
        public int UserAnswerTempId { get; set; }          // Benzersiz ID (temp tablosu için)

        [ForeignKey("UserQuizInfoTemp")]
        public int UserQuizInfoTempId { get; set; }        // Hangi UserQuizInfoTemp'e ait olduğunu belirtir
        public UserQuizInfoTemp UserQuizInfoTemp { get; set; }  // UserQuizInfoTemp ile ilişki

        public int QuestionId { get; set; }                // Hangi soruya ait olduğunu belirtir
        public Question Question { get; set; }             // Soru ile ilişki
 
        public int SelectedOptionId { get; set; }          // Kullanıcının seçtiği şıkkın ID'si
        public Option SelectedOption { get; set; }         // Seçilen şık ile ilişki

        public bool IsCorrect { get; set; }                // Cevap doğru mu, yanlış mı
    }
}
