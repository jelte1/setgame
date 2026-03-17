using Microsoft.Build.Framework;

namespace backend.Dtos.User;

public class RegisterLoginUserDto
{
    public required string UserName { get; set; }
    
    public required string PasswordHash { get; set; }
}