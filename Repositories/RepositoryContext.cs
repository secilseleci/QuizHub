using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repositories;

public class RepositoryContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<UserQuizInfo> UserQuizInfo { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Quiz>()
       .HasMany(q => q.Departments)
       .WithMany(d => d.Quizzes)
       .UsingEntity(j => j.ToTable("QuizDepartments"));

        modelBuilder.Entity<UserQuizInfo>()
        .HasMany(uqi => uqi.UserAnswers)
        .WithOne(ua => ua.UserQuizInfo)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserAnswer>()
        .HasOne(ua => ua.Question)
        .WithMany(q => q.UserAnswers)
        .OnDelete(DeleteBehavior.Restrict); 
                                                   
        modelBuilder.Entity<ApplicationUser>()
        .HasOne(u => u.DepartmentName)
        .WithMany(d => d.Users)
        .HasForeignKey(u => u.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Department>().HasData(
        new Department { DepartmentId = 1, DeparmentName = "IT" },
        new Department { DepartmentId = 2, DeparmentName = "HR" },
        new Department { DepartmentId = 3, DeparmentName = "Designer" });

    }


}
