using Microsoft.Build.Framework;

namespace backend.DTOs.User;

public class RegisterLoginUserDto
{
    public required string UserName { get; set; }
    
    public required string PasswordHash { get; set; }
}