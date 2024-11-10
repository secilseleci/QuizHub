using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repositories;

public class RepositoryContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<UserQuizInfo> UserQuizInfo { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<UserQuizInfoTemp> UserQuizInfoTemp { get; set; }
    public DbSet<UserAnswerTemp> UserAnswersTemp { get; set; }
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Question>()
           .HasOne(q => q.Quiz)
           .WithMany(qz => qz.Questions)
           .HasForeignKey(q => q.QuizId)
           .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Option>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Departments)
            .WithMany(d => d.Quizzes)
            .UsingEntity(j => j.ToTable("QuizDepartments"));

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Department)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<UserQuizInfo>()
            .HasMany(uqi => uqi.UserAnswers)
            .WithOne(ua => ua.UserQuizInfo)
            .HasForeignKey(ua => ua.UserQuizInfoId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.Question)
            .WithMany(q => q.UserAnswers)
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);  

        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.SelectedOption)
            .WithMany(o => o.UserAnswers)  
            .HasForeignKey(ua => ua.SelectedOptionId)
            .OnDelete(DeleteBehavior.Restrict);
       
        // -- Temp Tablolar --

        
        modelBuilder.Entity<UserQuizInfoTemp>()
        .HasMany(uqiTemp => uqiTemp.UserAnswersTemp)
        .WithOne(uaTemp => uaTemp.UserQuizInfoTemp)
        .HasForeignKey(uaTemp => uaTemp.UserQuizInfoTempId)
        .OnDelete(DeleteBehavior.Cascade);  

      
        modelBuilder.Entity<UserAnswerTemp>()
        .HasOne(uaTemp => uaTemp.Question)
        .WithMany(q => q.UserAnswersTemp)
        .HasForeignKey(uaTemp => uaTemp.QuestionId)
        .OnDelete(DeleteBehavior.Restrict);  
        
        modelBuilder.Entity<UserAnswerTemp>()
       .HasOne(ua => ua.SelectedOption)
       .WithMany(o => o.UserAnswersTemp)   
       .HasForeignKey(ua => ua.SelectedOptionId)
       .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Department>().HasData(
        new Department { DepartmentId = 1, DepartmentName = "IT" },
        new Department { DepartmentId = 2, DepartmentName = "HR" },
        new Department { DepartmentId = 3, DepartmentName = "Designer" },
        new Department { DepartmentId = 4, DepartmentName = "General"});
    }


}
