using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class QuizDtoForUser
    {
    public int QuizId { get; set; }
    public string? Title { get; set; }
    public ICollection<Question> Questions { get; set; }
    public int QuestionCount { get; set; }
    }
}
