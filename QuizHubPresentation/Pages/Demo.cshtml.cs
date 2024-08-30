using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizHubPresentation.Infrastructure.Extensions;
using System;
namespace QuizHubPresentation.Pages
{
    public class DemoModel : PageModel
    {
        public String? FullName => HttpContext.Session.GetString("name") ?? "";

        public void OnGet()
        {
        }

        public void OnPost([FromForm] string name)
        {
            HttpContext.Session.SetString("name", name);
        }
    }
}
