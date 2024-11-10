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
            catch (Exception ex)
            {
                Log.Error($"Sistemsel hata meydana geldi: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Hata sayfasına yönlendir
            context.Response.Redirect("/Home/ErrorPage");
            return Task.CompletedTask;
        }
    }
}
 
