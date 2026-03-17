using backend.Entities;

namespace backend.Dtos.Card;

public class CreateCardDto
{
    public required Shape Shape { get; set; }
    
    public required Color Color { get; set; }
    
    public required Filling Filling { get; set; }
    
    public required Amount Amount { get; set; } 
}