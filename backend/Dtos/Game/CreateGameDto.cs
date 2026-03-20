using backend.Entities;

namespace backend.DTOs.Game;

public class CreateGameDto
{
    public required string UserId { get; set; }
}