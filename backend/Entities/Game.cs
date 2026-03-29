using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Game
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsFinished { get; set; }

    public required string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public ICollection<GameCardState> GameCardStates { get; set; } = new List<GameCardState>();

    public ICollection<FoundSet> FoundSets { get; set; } = new List<FoundSet>();
}