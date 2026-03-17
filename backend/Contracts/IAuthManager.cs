using backend.Dtos.User;
using backend.Entities;
using Microsoft.AspNetCore.Identity;

namespace backend.Contracts;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(RegisterLoginUserDto user);
    Task<AuthResponseDto> Login(RegisterLoginUserDto user);
    Task<string> CreateRefreshToken();
    Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);

}