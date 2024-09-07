using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class OptionDtoForInsertion
    {
        [Required(ErrorMessage = "Option text is required")]
        public string OptionText { get; set; }

        public bool IsCorrect { get; set; }  
        public int QuestionId { get; set; }
    }
}
