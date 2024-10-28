using Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizHubPresentation.Models;
using Repositories;
using Repositories.Contracts;
using Serilog.Events;
using Serilog;
using Services;
using Services.Contracts;

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
        public static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None; // None olarak ayarla
                options.Secure = CookieSecurePolicy.Always; // HTTPS için zorunlu
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }
        public static void ConfigureServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IQuizService, QuizManager>();
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IQuestionService, QuestionManager>();
            services.AddScoped<IOptionService, OptionManager>();
            services.AddScoped<IUserQuizInfoService, UserQuizInfoManager>();
            services.AddScoped<IUserAnswerService, UserAnswerManager>();
            services.AddScoped<IDepartmentService, DepartmentManager>();
            services.AddScoped<IUserQuizInfoTempService, UserQuizInfoTempManager>();
            services.AddScoped<IUserAnswerTempService, UserAnswerTempManager>();// Yeni eklediğimiz service
        }

        public static void ConfigureApplicationCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.AccessDeniedPath = new PathString("/Error/403");
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

        public static void ConfigureSerilog(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()  
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) 
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
