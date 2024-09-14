using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class QuizConfig : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            // Quiz için temel yapılandırma
            builder.HasKey(p => p.QuizId);
            builder.Property(p => p.Title).IsRequired();

            // Quiz ve Question arasında one-to-many ilişki
            builder.HasMany(q => q.Questions)
                   .WithOne(q => q.Quiz)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.Cascade); // Quiz silinirse, sorular da silinir

            // Seed data - Quiz
            builder.HasData(
                new Quiz
                {
                    QuizId = 1,
                    Title = "World Capitals Quiz1",
                    ShowCase = true,
                    QuestionCount = 4
                },
                new Quiz
                {
                    QuizId = 2,
                    Title = "World Capitals Quiz2",
                    ShowCase = true,
                    QuestionCount = 2
                },
                new Quiz
                {
                    QuizId = 3,
                    Title = "World Capitals Quiz3",
                    ShowCase = true,
                    QuestionCount = 2
                }
            );
        }
    }
}
