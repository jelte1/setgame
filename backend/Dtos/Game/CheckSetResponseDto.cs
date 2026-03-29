using backend.Dtos.GameCardState;

namespace backend.DTOs.Game;

public class CheckSetResponseDto
{
    public bool IsSet { get; set; }
    public ICollection<GetGameCardStateDto> NewGameCardStates { get; set; } = new List<GetGameCardStateDto>();
}