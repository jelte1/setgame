using Microsoft.AspNetCore.Identity;

namespace backend.Entities;

public class User : IdentityUser
{
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}