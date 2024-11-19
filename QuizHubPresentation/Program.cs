using QuizHubPresentation.Infrastructure.Extensions;
using QuizHubPresentation.Infrastructure.Middleware;
using Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
           .WriteTo.Console()
           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
});

// Servis Extension
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSession();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.ConfigureRouting();
builder.Services.ConfigureApplicationCookie();
builder.Services.AddAutoMapper(typeof(Program));

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


// Ortam Belirleme
if (app.Environment.IsDevelopment())
{
    // Geliþtirme ortamýna özel ayarlar
    Console.WriteLine("Application running in Development mode.");
}
else if (app.Environment.IsProduction())
{
    // Yayýn ortamýna özel ayarlar
    Console.WriteLine("Application running in Production mode.");
}
// Middlewares
app.UseStaticFiles();
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

// ApplicationExtension
app.ConfigureAndCheckMigration();
app.ConfigureLocalization();
await app.ConfigureDefaultAdminUser();
await app.ConfigureDevelopmentUsers();
app.UseSerilogRequestLogging();

app.Run();
