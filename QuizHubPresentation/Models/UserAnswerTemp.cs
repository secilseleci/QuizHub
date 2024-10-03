namespace QuizHubPresentation.Models
{
    public class UserAnswerTemp
    {
        public int QuestionId { get; set; }  // Hangi soruya ait olduğunu belirtir
        public int? SelectedOptionId { get; set; }  // Kullanıcının seçtiği şıkkın ID'si
        public bool IsCorrect { get; set; }  // Cevabın doğru olup olmadığını belirtir
    }
}
