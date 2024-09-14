using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            // Question için temel yapılandırma
            builder.HasKey(q => q.QuestionId);
            builder.Property(q => q.QuestionText).IsRequired();

            // Question ve Option arasında one-to-many ilişki
            builder.HasMany(q => q.Options)
                   .WithOne(o => o.Question)
                   .HasForeignKey(o => o.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);  

             builder.HasData(
                new Question
                {
                    QuestionId = 1,
                    QuizId = 1,  
                    Order = 1,
                    QuestionText = "What is the capital of Japan?",
                    CorrectOptionId = 1
                },
                new Question
                {
                    QuestionId = 2,
                    QuizId = 1,  
                    Order = 2,
                    QuestionText = "What is the capital of France?",
                    CorrectOptionId = 5
                },
                 new Question
                 {
                     QuestionId = 3,
                     QuizId = 1,
                     Order = 3,
                     QuestionText = "What is the capital of America?",
                     CorrectOptionId = 10
                 },
                  new Question
                  {
                      QuestionId = 4,
                      QuizId = 1,
                      Order = 4,
                      QuestionText = "What is the capital of Kanada?",
                      CorrectOptionId = 15
                  },
                new Question
                {
                    QuestionId = 5,
                    QuizId = 2,
                    Order = 1,
                    QuestionText = "What is the capital of Turkey?",
                    CorrectOptionId =  18
                },

                new Question
                {
                    QuestionId = 6,
                    QuizId = 2,
                    Order = 2,
                    QuestionText = "What is the capital of Spain?",
                    CorrectOptionId = 23
                }, 
                
                
                
                new Question
                {
                    QuestionId = 7,
                    QuizId = 3,
                    Order = 1,
                    QuestionText = "What is the capital of Italy?",
                    CorrectOptionId = 26
                },
                new Question
                {
                    QuestionId = 8,
                    QuizId = 3,
                    Order = 2,
                    QuestionText = "What is the capital of England?",
                    CorrectOptionId = 32
                }
            );
        }
    }
}
