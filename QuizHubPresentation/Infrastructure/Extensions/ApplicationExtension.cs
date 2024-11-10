using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Serilog;

namespace QuizHubPresentation.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        {
            RepositoryContext context = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static void ConfigureLocalization(this WebApplication app)
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("tr-TR")
                    .AddSupportedUICultures("tr-TR")
                    .SetDefaultCulture("tr-TR");
            });
        }

        public static async Task ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminUser = "Admin";
            const string adminPassword = "Admin+123456";

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var admin = await userManager.FindByNameAsync(adminUser);

                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = adminUser,
                        Email = "admin@example.com",
                        DepartmentId = 1
                    };


                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        if (!await roleManager.RoleExistsAsync("Admin"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Admin"));
                        }

                        await userManager.AddToRoleAsync(admin, "Admin");
                    }

                    if (!result.Succeeded)
                        throw new Exception("Admin user could not been created.");
                }
        }

         
    }


        public static async Task ConfigureDevelopmentUsers(this IApplicationBuilder app)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                return;

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { "Editor", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var users = new List<ApplicationUser>
        {
                 new ApplicationUser { UserName = "AyselKara", Email = "aysel@example.com", DepartmentId = 1 },
                 new ApplicationUser { UserName = "SedaOyar", Email = "seda@example.com", DepartmentId = 2 },
                 new ApplicationUser { UserName = "CemSeker", Email = "cem@example.com", DepartmentId = 3 }
        };

                var passwords = new Dictionary<string, string>
        {
            { "AyselKara", "Aysel+123456" },
            { "SedaOyar", "Seda+123456" },
            { "CemSeker", "Cem+123456" }
        };

                foreach (var user in users)
                {
                    var existingUser = await userManager.FindByNameAsync(user.UserName);
                    if (existingUser == null)
                    {
                        var result = await userManager.CreateAsync(user, passwords[user.UserName]);
                        if (result.Succeeded)
                        {
                            // Tüm kullanıcılara "User" rolü atanıyor
                            await userManager.AddToRoleAsync(user, "User");
                        }
                    }
                }
            }
        }

    }
}