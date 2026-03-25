using backend.DTOs.User;
using backend.Entities;
using Microsoft.AspNetCore.Identity;

namespace backend.Interfaces;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(RegisterLoginUserDto user);
    Task<AuthResponseDto> Login(RegisterLoginUserDto user);
    Task<string> CreateRefreshToken();
    Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
}