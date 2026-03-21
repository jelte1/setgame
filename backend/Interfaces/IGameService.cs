using backend.DTOs.Game;
using backend.Entities;

namespace backend.Interfaces;

public interface IGameService
{
    Task<Game> CreateGameAsync(string userId);
    Task<CheckSetResponseDto> CheckSet(int gameId, CheckSetDto checkSetDto);
    Task<List<GameState>> DrawCards(Game game);
}