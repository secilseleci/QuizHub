using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Repositories.Config;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Repositories;

public class RepositoryContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<UserQuizInfo> UserQuizInfo { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }

    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
 
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<UserQuizInfo>()
      .HasMany(uqi => uqi.UserAnswers)
      .WithOne(ua => ua.UserQuizInfo)
      .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict kullan

        modelBuilder.Entity<UserAnswer>()
           .HasOne(ua => ua.Question)
           .WithMany(q => q.UserAnswers)
           .OnDelete(DeleteBehavior.Restrict); // Diğer ilişkilerde de aynı şekilde davran

    }


}
