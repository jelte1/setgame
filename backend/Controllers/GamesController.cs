using backend.Contracts;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGamesRepository _gamesRepository;

        public GamesController(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        // GET: api/Game
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        // {
        //     return await _gamesRepository.GetAllAsync();
        // }

        // GET: api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gamesRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Game/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutGame(int id, Game game)
        // {
        //     if (id != game.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _gamesRepository.Entry(game).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _gamesRepository.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!GameExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }

        // POST: api/Game
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            await _gamesRepository.AddAsync(game);

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _gamesRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            await _gamesRepository.DeleteAsync(id);

            return NoContent();
        }

        // GET: api/Game/1/user
        [HttpGet("{id}/user")]
        public async Task<ActionResult<User>> GetGameUser(int id)
        {
            var game = await _gamesRepository.GetAsync(id);
            var user = game.User;

            if (game == null || user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        private async Task<bool> GameExists(int id)
        {
            return await _gamesRepository.Exists(id);
        }
    }
}