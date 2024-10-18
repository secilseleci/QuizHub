namespace QuizHubPresentation.Models
{
    public class QuizResultViewModel
    {
        public string QuizTitle { get; set; }
        public int QuestionCount { get; set; }
        public int CorrectAnswer { get; set; }
        public int FalseAnswer { get; set; }
        public int Score { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
