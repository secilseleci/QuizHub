using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuizHubPresentation.Models
{
    public class AssignQuizViewModel
    {
        public int QuizId { get; set; }  // Quiz ID
        public string QuizTitle { get; set; }  // Quiz Başlığı

        public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();

        // Seçilen departman
        public string SelectedDepartment { get; set; }
    }
}
