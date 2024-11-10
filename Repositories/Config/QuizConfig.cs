using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class QuizConfig : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(p => p.QuizId);
            builder.Property(p => p.Title).IsRequired();

            builder.HasMany(q => q.Questions)
                   .WithOne(q => q.Quiz)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Quiz
                {
                    QuizId = 1,
                    Title = "HR Basics",
                    ShowCase = true,
                    QuestionCount = 5
                },
                new Quiz
                {
                    QuizId = 2,
                    Title = "IT Fundamentals",
                    ShowCase = true,
                    QuestionCount = 5
                },
                new Quiz
                {
                    QuizId = 3,
                    Title = "Design Principles",
                    ShowCase = true,
                    QuestionCount = 5
                },
                new Quiz
                {
                    QuizId = 4,
                    Title = "Geography Trivia",
                    ShowCase = true,
                    QuestionCount = 5
                },
                new Quiz
                {
                    QuizId = 5,
                    Title = "Historical Events",
                    ShowCase = true,
                    QuestionCount = 5
                },
                new Quiz
                {
                    QuizId = 7,
                    Title = "Türk Sineması Bilgi Yarışması",
                    ShowCase = true,
                    QuestionCount = 5
                }

            );
        }
    }
}
