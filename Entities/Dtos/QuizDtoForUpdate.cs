namespace Entities.Dtos
{
    public class QuizDtoForUpdate 
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public int QuestionCount { get; set; }
        public bool ShowCase { get; set; }
        public DateTime UpdatedDate { get; set; }= DateTime.Now;

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
