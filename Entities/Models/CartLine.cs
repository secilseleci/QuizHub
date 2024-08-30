namespace Entities.Models;

public class CartLine
{
    public int CartLineId { get; set; }
    public Quiz Quiz { get; set; } = new();
    public int Quantity { get; set; }
}