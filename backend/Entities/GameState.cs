using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public enum CardLocation { Deck, Table, Discarded }

public class GameState
{
    public int Id { get; set; }
    
    public CardLocation Location { get; set; }
    
    public int Order { get; set; }

    public int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    [Required]
    public Game Game { get; set; }
    
    public int CardId { get; set; }
    [ForeignKey(nameof(CardId))]
    [Required]
    public Card Card { get; set; }
}