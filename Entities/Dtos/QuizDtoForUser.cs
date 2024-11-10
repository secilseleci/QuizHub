
namespace Entities.Dtos
{
    public class QuizDtoForUser
    {
    public int QuizId { get; set; }
    public string? Title { get; set; }
    public IList<QuestionDto> Questions { get; set; }
    public int QuestionCount { get; set; }
    public bool ShowFinishButton { get; set; }

    }
}
