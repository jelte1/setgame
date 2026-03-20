using backend.Entities;

namespace backend.Dtos.FoundSet;

public class GetFoundSetDto
{
    public int Id { get; set; }
    
    public required Card Card1 { get; set; }
    
    public required Card Card2 { get; set; }
    
    public required Card Card3 { get; set; }
}