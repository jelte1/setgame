namespace backend.DTOs.Game;

public class GetBaseGameDto
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsFinished { get; set; }
}