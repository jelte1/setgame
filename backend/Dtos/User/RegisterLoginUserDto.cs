using Microsoft.Build.Framework;

namespace backend.Dtos.User;

public class RegisterLoginUserDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string PasswordHash { get; set; }
}