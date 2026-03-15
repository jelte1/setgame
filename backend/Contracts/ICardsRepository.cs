using backend.Entities;

namespace backend.Contracts;

public interface ICardsRepository : IGenericRepository<Card>
{
    Task<List<Card>> GetCardsByIds(List<int> ids);
}