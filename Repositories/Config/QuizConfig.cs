using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config;

public class QuizConfig : IEntityTypeConfiguration<Quiz>
{

    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(p => p.QuizId);
        builder.Property(p => p.Title).IsRequired();

        builder.HasData(
        new Quiz() { QuizId = 1, Title = "İSG", ShowCase = true },
        new Quiz() { QuizId = 2, Title = "KVKK", ShowCase = false },
        new Quiz() { QuizId = 3, Title = "Bilgi Güvenliği", ShowCase = true },
        new Quiz() { QuizId = 4, Title = "Çevre", ShowCase = true }
        );
    }
}