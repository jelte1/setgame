using backend.Entities;
using backend.Interfaces;

namespace backend.Services;

public class SetValidationService : ISetValidationService
{
    public bool IsValidSet(Card card1, Card card2, Card card3)
    {
        return IsValidProperty((int)card1.Color,   (int)card2.Color,   (int)card3.Color)
               && IsValidProperty((int)card1.Shape,   (int)card2.Shape,   (int)card3.Shape)
               && IsValidProperty((int)card1.Filling, (int)card2.Filling, (int)card3.Filling)
               && IsValidProperty((int)card1.Amount,  (int)card2.Amount,  (int)card3.Amount);
    }
    
    private bool IsValidProperty(int a, int b, int c)
    {
        var uniqueValues = new HashSet<int> { a, b, c };
        return uniqueValues.Count != 2;
    }

    public List<(Card, Card, Card)> FindAllSets(List<Card> tableCards)
    {
        var result = new List<(Card, Card, Card)>();

        // Get all combinations of 3 cards and check for set
        for (int i = 0; i < tableCards.Count - 2; i++)
        {
            for (int j = i + 1; j < tableCards.Count - 1; j++)
            {
                for (int k = j + 1; k < tableCards.Count; k++)
                {
                    var card1 = tableCards[i];
                    var card2 = tableCards[j];
                    var card3 = tableCards[k];

                    if (IsValidSet(card1, card2, card3))
                    {
                        result.Add((card1, card2, card3));
                    }
                }
            }
        }

        return result;
    }

    public (Card, Card, Card)? FindHint(List<Card> tableCards)
    {
        var allSets = FindAllSets(tableCards);
        return allSets.FirstOrDefault();
    }
    
    // public EnsureSetsPossible(List<Card> tableCards)
    // {
    //     while (FindAllSets(tableCards).Count == 0)
    //     {
    //         // Logic to add more cards to the table until at least one set is possible
    //         // This would typically involve drawing cards from the deck and adding them to the table
    //     }
    // }
}
