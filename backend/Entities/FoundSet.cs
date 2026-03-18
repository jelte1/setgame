using System.ComponentModel.DataAnnotations.Schema;
using backend.Entities;
using Microsoft.Build.Framework;

namespace backend.Entities;

public class FoundSet
{
    public int Id { get; set; }

    public int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    [Required]
    public required Game Game { get; set; }

    public int Card1Id { get; set; }
    [ForeignKey(nameof(Card1Id))]
    [Required]
    public required Card Card1 { get; set; }

    public int Card2Id { get; set; }
    [ForeignKey(nameof(Card2Id))]
    [Required]
    public required Card Card2 { get; set; }

    public int Card3Id { get; set; }
    [ForeignKey(nameof(Card3Id))]
    [Required]
    public required Card Card3 { get; set; }
}