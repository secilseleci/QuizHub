using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class OptionDto
    {
        public int OptionId { get; set; }
        [Required(ErrorMessage = "Option text is required")]
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }  
        public int QuestionId { get; set; }
        public bool IsDisabled { get; set; }

        public bool IsSelected { get; set; }


    }
}
