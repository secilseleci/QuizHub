using Microsoft.Extensions.Logging;
using QuizHubPresentation.Infrastructure.Extensions;
using QuizHubPresentation.Infrastructure.Middleware;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


builder.Services.ConfigureSerilog();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews(); 
builder.Services.AddRazorPages();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSession();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.ConfigureRouting();
builder.Services.ConfigureApplicationCookie();
builder.Services.AddAutoMapper(typeof(Program));
 

builder.Host.UseSerilog();
var app = builder.Build();

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


app.ConfigureAndCheckMigration();
app.ConfigureLocalization();
await app.ConfigureDefaultAdminUser();
await app.ConfigureDevelopmentUsers(); 
app.Run();
