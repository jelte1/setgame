using backend.Dtos.GameState;

namespace backend.DTOs.Game;

public class CheckSetResponseDto
{
    public bool IsSet { get; set; }
    public ICollection<GetGameStateDto> NewGameStates { get; set; } = new List<GetGameStateDto>();
}