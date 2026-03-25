using backend.Entities;

namespace backend.Interfaces;

public interface ISetValidationService
{
    bool IsValidSet(Card card1, Card card2, Card card3);
    List<(Card, Card, Card)> FindAllSets(List<Card> tableCards);
    (Card, Card, Card)? FindHint(List<Card> tableCards);
}