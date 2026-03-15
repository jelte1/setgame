using backend.Entities;

namespace backend.Dtos.Card;

public class GetCardDto
{
    public int Id { get; set; }
    
    public Shape Shape { get; set; }
    
    public Color Color { get; set; }
    
    public Filling Filling { get; set; }
    
    public Amount Amount { get; set; } 
}