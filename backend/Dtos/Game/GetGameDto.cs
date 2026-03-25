using backend.Dtos.FoundSet;
using backend.Dtos.GameState;
using backend.Entities;

namespace backend.DTOs.Game;

public class GetGameDto
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsFinished { get; set; }
    
    public string UserId { get; set; }
    
    public ICollection<GetGameStateDto> GameStates { get; set; } = new List<GetGameStateDto>();

    public ICollection<GetFoundSetDto> FoundSets { get; set; } = new List<GetFoundSetDto>();
}