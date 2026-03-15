using backend.Entities;

namespace backend.Entities;

public class Set
{
    public int Id { get; set; }
    
    public ICollection<Card> Cards { get; set; }
}