using backend.DTOs.Card;
using backend.Entities;

namespace backend.Dtos.GameCardState;

public class GetGameCardStateDto
{
    public int Id { get; set; }
    
    public CardLocation Location { get; set; }
    
    public int Order { get; set; }

    public GetCardDto Card { get; set; }
}