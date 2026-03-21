using System.ComponentModel.DataAnnotations.Schema;
using backend.Entities;
using Microsoft.Build.Framework;

namespace backend.Entities;

public class FoundSet
{
    public int Id { get; set; }

    public required int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public Game Game { get; set; }

    public required int Card1Id { get; set; }
    [ForeignKey(nameof(Card1Id))]
    public Card Card1 { get; set; }

    public required int Card2Id { get; set; }
    [ForeignKey(nameof(Card2Id))]
    public Card Card2 { get; set; }

    public required int Card3Id { get; set; }
    [ForeignKey(nameof(Card3Id))]
    public Card Card3 { get; set; }
}