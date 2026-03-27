using backend.Entities;

namespace backend.Interfaces;

public interface IGamesRepository : IGenericRepository<Game>
{
    Task<Game?> GetGameWithUserAsync(int id);
    Task<List<Game>> GetGamesByUserAsync(string userId);
    Task<Game?> GetGameWithStatesAsync(int id);
    Task<Game?> GetGameWithStatesAndFoundSetsAsync(int id);
}