using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class Game
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsFinished { get; set; }

    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    [Required]
    public required User User { get; set; }

    public ICollection<GameState> GameStates { get; set; } = new List<GameState>();

    public ICollection<FoundSet> FoundSets { get; set; } = new List<FoundSet>();
}