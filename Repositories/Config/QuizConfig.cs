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

     
    }
}