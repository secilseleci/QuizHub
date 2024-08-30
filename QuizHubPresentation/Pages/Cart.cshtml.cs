using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Contracts;

namespace QuizHubPresentation.Pages
{
    public class CartModel : PageModel
    {
        private readonly IServiceManager _manager;

        public Cart Cart { get; set; } // IoC
        public string ReturnUrl { get; set; } = "/";

        public CartModel(IServiceManager manager, Cart cartService)
        {
            _manager = manager;
            Cart = cartService;
        }


        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(int quizId, string returnUrl)
        {
            Quiz? quiz = _manager
                .QuizService
                .GetOneQuiz(quizId, false);

            if (quiz is not null)
            {
                Cart.AddItem(quiz, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl }); 
        }

        public IActionResult OnPostRemove(int id, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Quiz.QuizId.Equals(id)).Quiz);
            return Page();
        }
    }
}