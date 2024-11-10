using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class QuestionDto
    {
        public int Order { get; set; }
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Question text is required")]
        public string QuestionText { get; set; }
        public int QuestionCount { get; set; }

        public int CorrectOptionId { get; set; }
        public int QuizId { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
      public bool IsLastQuestion {  get; set; }
    }
}
