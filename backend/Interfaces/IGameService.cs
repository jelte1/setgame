using backend.DTOs.Game;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces;

public interface IGameService
{
    Task<Game> CreateGameAsync(string userId);
    Task<CheckSetResponseDto> CheckSet(int gameId, CheckSetDto checkSetDto);
    Task<List<GameCardState>> DrawCards(Game game, int amount = 3);
    (Card, Card, Card)? GetHint(Game game);
}