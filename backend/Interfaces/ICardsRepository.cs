using backend.Entities;

namespace backend.Interfaces;

public interface ICardsRepository : IGenericRepository<Card>
{
    Task<List<Card>> GetCardsByIds(List<int> ids);
}