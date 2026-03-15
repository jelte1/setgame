namespace backend.Entities;

public class Board
{
    public int Id { get; set; }
    
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}