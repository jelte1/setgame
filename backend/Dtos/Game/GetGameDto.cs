using backend.Dtos.FoundSet;
using backend.Dtos.GameCardState;
using backend.Entities;

namespace backend.DTOs.Game;

public class GetGameDto
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsFinished { get; set; }
    
    public string UserId { get; set; }
    
    public ICollection<GetGameCardStateDto> GameCardStates { get; set; } = new List<GetGameCardStateDto>();

    public ICollection<GetFoundSetDto> FoundSets { get; set; } = new List<GetFoundSetDto>();
}