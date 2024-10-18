using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class UserQuizInfoTemp
    {
        public int UserQuizInfoTempId { get; set; }  // Benzersiz ID (temp tablosu için)

        public string UserId { get; set; }           // Kullanıcının ID'si
        public ApplicationUser User { get; set; }    // Kullanıcı ile ilişki

        public int QuizId { get; set; }              // Hangi quiz'e ait olduğunu belirtir
        public Quiz Quiz { get; set; }               // Quiz ile ilişki

        public bool IsCompleted { get; set; }        // Quiz tamamlandı mı
        public DateTime? StartedAt { get; set; }     // Quiz'in başladığı zaman
        public DateTime? LastUpdated { get; set; }   // En son ne zaman güncellendiği (ilerlemeyi takip etmek için)

        public int CorrectAnswer { get; set; }       // Doğru cevap sayısı (quiz tamamlanınca esas tabloya taşınacak)
        public int FalseAnswer { get; set; }         // Yanlış cevap sayısı (quiz tamamlanınca esas tabloya taşınacak)
        public int Score { get; set; }    // Yeni alan

        public ICollection<UserAnswerTemp> UserAnswersTemp { get; set; }  // Temp cevaplarla ilişki
    }
}
