using Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Repositories.Implementations;

using Services;
using Services.Contracts;
using Services.Implemantations;
using Services.Implementations;

namespace QuizHubPresentation.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("sqlconnection"),
                    b => b.MigrationsAssembly("Repositories"));

                options.EnableSensitiveDataLogging(true);
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "QuizHub.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void ConfigureRepositoryRegistration(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IOptionRepository, OptionRepository>();
            services.AddScoped<IUserQuizInfoRepository, UserQuizInfoRepository>();
            services.AddScoped<IUserAnswerRepository, UserAnswerRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserQuizInfoTempRepository, UserQuizInfoTempRepository>();
            services.AddScoped<IUserAnswerTempRepository, UserAnswerTempRepository>();// Yeni eklediğimiz repository
        }
      
        public static void ConfigureServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IOptionService, OptionService>();
            services.AddScoped<IUserQuizInfoService, UserQuizInfoService>();
            services.AddScoped<IUserAnswerService, UserAnswerService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserQuizInfoTempService, UserQuizInfoTempService>();
            services.AddScoped<IUserAnswerTempService, UserAnswerTempService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            services.AddTransient<IEmailSender, EmailSender>();
        }

        public static void ConfigureApplicationCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.AccessDeniedPath = new PathString("/Account/Login");
            });
        }

        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });
        }
        public static void ConfigureMvcWithGlobalAuthorization(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                // Global Authorization Filter
                options.Filters.Add(new AuthorizeFilter());
            }); 
            services.AddRazorPages(options =>
            {
                // Razor Pages istisnalarını açıkça tanımla
                options.Conventions.AllowAnonymousToPage("/About");
                options.Conventions.AllowAnonymousToPage("/Contact");
            });
        }

    }
}
