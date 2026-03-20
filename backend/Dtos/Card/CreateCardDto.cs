using backend.Entities;

namespace backend.DTOs.Card;

public class CreateCardDto
{
    public Shape Shape { get; set; }
    
    public Color Color { get; set; }
    
    public Filling Filling { get; set; }
    
    public Amount Amount { get; set; } 
}