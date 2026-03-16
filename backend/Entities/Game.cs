using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Game
{
    public int Id { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public required User User { get; set; }
    
    public required Board Board { get; set; }
    
    public virtual ICollection<Card> CardsInBacklog { get; set; }
    
    public ICollection<Set> FoundSets { get; set; }
    
    public bool IsFinished { get; set; }
}