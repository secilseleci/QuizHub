namespace Entities.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public int QuestionCount { get; set; }
        public bool ShowCase { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Question> Questions { get; set; }

    }
}
