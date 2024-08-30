namespace Entities.Models
{


    public class Cart
    {
        public List<CartLine> Lines { get; set; }
        public Cart()
        {
            Lines = new List<CartLine>();
        }

        public virtual void AddItem(Quiz quiz, int quantity)
        {
            CartLine? line = Lines.Where(l => l.Quiz.QuizId == quiz.QuizId).FirstOrDefault();
            if (line is null)
            {
                Lines.Add(new CartLine()
                {
                    Quiz = quiz,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Quiz quiz) =>
        Lines.RemoveAll(l => l.Quiz.QuizId == quiz.QuizId);

      
        public virtual void Clear() => Lines.Clear();
    }
}