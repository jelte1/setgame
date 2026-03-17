using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.User;

public class AuthResponseDto
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}