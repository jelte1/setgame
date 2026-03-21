using backend.Entities;

namespace backend.Interfaces;

public interface IGamesRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameWithUserAsync(int id);
    Task<Game?> GetGameWithStatesAsync(int id);
    Task<Game?> GetGameWithStatesAndFoundSetsAsync(int id);
}