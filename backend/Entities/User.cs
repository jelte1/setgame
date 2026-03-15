namespace backend.Entities;

public class User
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string PasswordHash { get; set; }
    
    public virtual ICollection<Game> Games { get; set; }
}