using System;
using System.Linq;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace QuizHubPresentation.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;
        public CartSummaryViewComponent(Cart cartService)
        {
            _cart = cartService;
        }

        public string Invoke()
        {
            int totalItems = _cart.Lines.Sum(line => line.Quantity);
            return totalItems.ToString();

        }
    }
}