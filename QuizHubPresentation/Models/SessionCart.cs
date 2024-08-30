using Entities.Models;
using QuizHubPresentation.Infrastructure.Extensions;
using System.Text.Json.Serialization;

namespace QuizHubPresentation.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {

            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;

            SessionCart cart = session?.GetJson<SessionCart>("cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        public override void AddItem(Quiz quiz, int quantity)
        {
            base.AddItem(quiz, quantity);
            Session?.SetJson<SessionCart>("cart", this);
        }
    }
}