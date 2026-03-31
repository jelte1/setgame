using backend.Dtos.FoundSet;
using backend.Dtos.GameCardState;
using backend.Entities;

namespace backend.DTOs.Game;

public class CheckSetResponseDto
{
    public bool IsSet { get; set; }
    public ICollection<GetGameCardStateDto> NewGameCardStates { get; set; } = new List<GetGameCardStateDto>();
    public GetFoundSetDto? FoundSet { get; set; }
}