using backend.Entities;
using backend.Interfaces;

namespace backend.Services;

public class SetValidationService : ISetValidationService
{
    public bool IsValidSet(Card card1, Card card2, Card card3)
    {
        if (IsValidProperty((int)card1.Color, (int)card2.Color, (int)card3.Color) 
           && 
           IsValidProperty((int)card1.Shape, (int)card2.Shape, (int)card3.Shape) 
           && 
           IsValidProperty((int)card1.Filling, (int)card2.Filling, (int)card3.Filling) 
           && 
           IsValidProperty((int)card1.Amount, (int)card2.Amount, (int)card3.Amount))
        {
            return true;
        }
        
        return false;
    }

    private bool IsValidProperty(int a, int b, int c)
    {
        // either the specific property of every card is the same or different for all 3 cards
        if ((a == b && b == c && a == c)
            ||
            (a != b && b != c && a != c))
        {
            return true;
        }

        return false;
    }

    public List<(Card, Card, Card)> FindAllSets(List<Card> tableCards)
    {
        // eventual results of all possible sets
        var result = new List<(Card, Card, Card)>();
        // current selection of cards will be checked for if it is a set
        var currentSelection = new List<Card>();
        
        // using a backtracking algorithm to get all the possible sets.
        BacktrackCards(result, currentSelection, tableCards, 0);

        return result;
    }

    private void BacktrackCards(List<(Card, Card, Card)> result, List<Card> currentSelection, List<Card> cards, int index)
    {
        // if there are 3 cards in the current selection; check if they form a set
        // if so that set gets added to the total result list of sets
        if (currentSelection.Count == 3)
        {
            if (IsValidSet(currentSelection[0], currentSelection[1], currentSelection[2]))
            {
                result.Add((currentSelection[0], currentSelection[1], currentSelection[2]));
            }
            
            // break out of the current path since there cant be more then 3 cards
            return;
        }
        
        // loop through all of the cards from the current index to the end of the list
        // this goes recursively through all the possible combinations of cards
        for (; index < cards.Count; index++)
        {
            // add card with current index to the selection
            currentSelection.Add(cards[index]);
            
            // continue to add more cards to the selection until there are 3 cards in the selection and then check if they form a set
            BacktrackCards(result, currentSelection, cards, index + 1);
            
            // remove the last card from the selection so it can try the next card in the loop. 
            currentSelection.RemoveAt(currentSelection.Count - 1);
        }
    }

    public (Card, Card, Card)? FindHint(List<Card> tableCards)
    {
        var allSets = FindAllSets(tableCards);
        
        return allSets.FirstOrDefault();
    }
    
}