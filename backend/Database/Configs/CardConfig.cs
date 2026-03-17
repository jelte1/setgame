using backend.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Database.Configs;

public class CardConfig : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasData(CreateAllCards());
    }
    
    private ICollection<Card> CreateAllCards()
    {
        ICollection<Card> cards = new List<Card>();
        int id = 1;
        
        foreach (var shape in Enum.GetValues<Shape>())
        {
            foreach (var color in Enum.GetValues<Color>())
            {
                foreach (var filling in Enum.GetValues<Filling>())
                {
                    foreach (var amount in Enum.GetValues<Amount>())
                    {
                        cards.Add(new Card
                        {
                            Id = id++,
                            Shape = shape,
                            Color = color,
                            Filling = filling,
                            Amount = amount
                        });
                    }
                }
            } 
        }
        return cards;
    }
}