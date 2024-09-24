using Entities.Models;
namespace QuizHubPresentation.Models
{
    public class QuizListViewModel
    {
        public IEnumerable<Quiz> Quizzes { get; set; } = Enumerable.Empty<Quiz>();
        public Pagination Pagination { get; set; } = new();

    }
}



 
    
 