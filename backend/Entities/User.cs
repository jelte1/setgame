using Microsoft.AspNetCore.Identity;

namespace backend.Entities;

public class User : IdentityUser
{
    public override string Id { get; set; }
    public virtual ICollection<Game>? Games { get; set; } = new List<Game>();
}