using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities;

public class Game
{
    public int Id { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }
    
    public required User User { get; set; }
    
    [ForeignKey(nameof(BoardId))]
    public int BoardId { get; set; }
    
    public required Board Board { get; set; }
    
    public virtual Board CardsInBacklog { get; set; }
    
    public ICollection<Set> FoundSets { get; set; }
    
    public bool IsFinished { get; set; }
}