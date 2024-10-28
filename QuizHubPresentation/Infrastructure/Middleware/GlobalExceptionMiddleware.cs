using Entities.Middleware;
using Serilog;

namespace QuizHubPresentation.Infrastructure.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)  // NotFoundException yakala
            {
                // Hata loglanıyor
                Log.Error(ex, "Quiz bulunamadı! URL: {Url}, Method: {Method}, User: {User}",
                          context.Request.Path,
                          context.Request.Method,
                          context.User.Identity?.Name ?? "Anonymous");

                // 404 sayfasına yönlendir
                context.Response.Redirect("/Error/404");
            }
            catch (UnauthorizedAccessException ex) // UnauthorizedAccessException yakala
            {
                Log.Error(ex, "Yetkisiz erişim. URL: {Url}, Method: {Method}, User: {User}",
                          context.Request.Path,
                          context.Request.Method,
                          context.User.Identity?.Name ?? "Anonymous");

                // 403 sayfasına yönlendir
                context.Response.Redirect("/Error/403");
            }
            catch (Exception ex)  // Diğer tüm hatalar için
            {
                // Genel bir hata loglama ve yönlendirme yapıyoruz
                Log.Error(ex, "Sunucu hatası. URL: {Url}, Method: {Method}, User: {User}",
                          context.Request.Path,
                          context.Request.Method,
                          context.User.Identity?.Name ?? "Anonymous");

                context.Response.Redirect("/Error/500");  // Genel bir sunucu hatası sayfasına yönlendir
            }
        }
    }
}
