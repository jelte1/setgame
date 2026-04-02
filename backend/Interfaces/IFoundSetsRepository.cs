using backend.Entities;

namespace backend.Interfaces;

public interface IFoundSetsRepository : IGenericRepository<FoundSet>
{
    Task<bool> ExistsSet(int gameId, int cardId1, int cardId2, int cardId3);
}