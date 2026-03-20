using backend.Entities;

namespace backend.Interfaces;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<List<Game>?> GetUserGames(string userId);
    Task<User?> GetUserByUserId(string userId);
}