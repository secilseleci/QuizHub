using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.QuestionId);
            builder.Property(q => q.QuestionText).IsRequired();

            builder.HasMany(q => q.Options)
                   .WithOne(o => o.Question)
                   .HasForeignKey(o => o.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                // HR Basics Quiz Questions
                new Question { QuestionId = 1, QuizId = 1, Order = 1, QuestionText = "What does HR stand for?", CorrectOptionId = 1 },
                new Question { QuestionId = 2, QuizId = 1, Order = 2, QuestionText = "What is the primary role of HR?", CorrectOptionId = 5 },
                new Question { QuestionId = 3, QuizId = 1, Order = 3, QuestionText = "Which function is not typically part of HR?", CorrectOptionId = 9 },
                new Question { QuestionId = 4, QuizId = 1, Order = 4, QuestionText = "What is talent acquisition?", CorrectOptionId = 13 },
                new Question { QuestionId = 5, QuizId = 1, Order = 5, QuestionText = "What does HR primarily deal with?", CorrectOptionId = 17 },

                // IT Fundamentals Quiz Questions
                new Question { QuestionId = 6, QuizId = 2, Order = 1, QuestionText = "Which language is used for web development?", CorrectOptionId = 21 },
                new Question { QuestionId = 7, QuizId = 2, Order = 2, QuestionText = "What is RAM?", CorrectOptionId = 25 },
                new Question { QuestionId = 8, QuizId = 2, Order = 3, QuestionText = "What does IP stand for?", CorrectOptionId = 29 },
                new Question { QuestionId = 9, QuizId = 2, Order = 4, QuestionText = "Which is an operating system?", CorrectOptionId = 33 },
                new Question { QuestionId = 10, QuizId = 2, Order = 5, QuestionText = "What is cybersecurity?", CorrectOptionId = 37 },

                // Design Principles Quiz Questions
                new Question { QuestionId = 11, QuizId = 3, Order = 1, QuestionText = "What is a color wheel?", CorrectOptionId = 41 },
                new Question { QuestionId = 12, QuizId = 3, Order = 2, QuestionText = "Which of these is a design principle?", CorrectOptionId = 45 },
                new Question { QuestionId = 13, QuizId = 3, Order = 3, QuestionText = "What does UX stand for?", CorrectOptionId = 49 },
                new Question { QuestionId = 14, QuizId = 3, Order = 4, QuestionText = "What is whitespace?", CorrectOptionId = 53 },
                new Question { QuestionId = 15, QuizId = 3, Order = 5, QuestionText = "Which software is used for graphic design?", CorrectOptionId = 57 },

                // Geography Trivia Quiz Questions
                new Question { QuestionId = 16, QuizId = 4, Order = 1, QuestionText = "Which is the largest continent?", CorrectOptionId = 61 },
                new Question { QuestionId = 17, QuizId = 4, Order = 2, QuestionText = "What is the smallest country?", CorrectOptionId = 65 },
                new Question { QuestionId = 18, QuizId = 4, Order = 3, QuestionText = "What is the longest river?", CorrectOptionId = 69 },
                new Question { QuestionId = 19, QuizId = 4, Order = 4, QuestionText = "Which desert is the largest?", CorrectOptionId = 73 },
                new Question { QuestionId = 20, QuizId = 4, Order = 5, QuestionText = "What is the capital of Australia?", CorrectOptionId = 77 },

                // Historical Events Quiz Questions
                new Question { QuestionId = 21, QuizId = 5, Order = 1, QuestionText = "When did World War II end?", CorrectOptionId = 81 },
                new Question { QuestionId = 22, QuizId = 5, Order = 2, QuestionText = "Who discovered America?", CorrectOptionId = 85 },
                new Question { QuestionId = 23, QuizId = 5, Order = 3, QuestionText = "What was the Cold War?", CorrectOptionId = 89 },
                new Question { QuestionId = 24, QuizId = 5, Order = 4, QuestionText = "Who was the first President of the USA?", CorrectOptionId = 93 },
                new Question { QuestionId = 25, QuizId = 5, Order = 5, QuestionText = "Which year did the Berlin Wall fall?", CorrectOptionId = 97 },

                // Türk Sineması Soruları
                new Question { QuestionId = 31, QuizId = 7, Order = 1, QuestionText = "Türk sinemasında 'Çiçek Abbas' karakterini kim canlandırmıştır?", CorrectOptionId = 121 },
                new Question { QuestionId = 32, QuizId = 7, Order = 2, QuestionText = "Türk sinemasında 'Sultan' lakaplı oyuncu kimdir?", CorrectOptionId = 125 },
                new Question { QuestionId = 33, QuizId = 7, Order = 3, QuestionText = "Türk sinemasında en çok izlenen film hangisidir?", CorrectOptionId = 129 },
                new Question { QuestionId = 34, QuizId = 7, Order = 4, QuestionText = "'Hababam Sınıfı' filminin yazarı kimdir?", CorrectOptionId = 133 },
                new Question { QuestionId = 35, QuizId = 7, Order = 5, QuestionText = "'Eşkıya' filmi hangi yıl vizyona girmiştir?", CorrectOptionId = 137 }

            );
        }
    }
}
