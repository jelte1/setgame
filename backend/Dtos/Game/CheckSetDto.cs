namespace backend.DTOs.Game;

public class CheckSetDto
{
    public int Card1Id { get; set; }
    public int Card2Id { get; set; }
    public int Card3Id { get; set; }
    
    public List<int> ToList()
    {
        return new List<int> { Card1Id, Card2Id, Card3Id };
    }
}