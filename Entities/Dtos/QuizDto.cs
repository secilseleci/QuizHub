using Entities.Models;

namespace Entities.Dtos
{
    public class QuizDto 
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QuestionCount { get; set; }
        public bool ShowCase { get; set; }
        public ICollection<QuestionDto> Questions { get; set; }
        
    }
}