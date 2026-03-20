using System.Security.Claims;
using AutoMapper;
using backend.DTOs.Game;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IGameService _gamesService;
        private readonly IMapper _mapper;

        public GamesController(IGamesRepository gamesRepository, IUsersRepository usersRepository, IGameService gameService, IMapper mapper)
        {
            _gamesRepository = gamesRepository;
            _usersRepository = usersRepository;
            _gamesService = gameService;
            _mapper = mapper;
        }

        // GET: api/Game/5
        [HttpGet("{id}")]
        [Authorize]
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
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetGameDto>> PostGame()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            var user = await _usersRepository.GetUserByUserId(userId);
            
            if (user == null)
            {
                return Unauthorized();
            }
            
            var game = await _gamesService.CreateGameAsync(userId);
            
            var getGameDto = _mapper.Map<GetGameDto>(game);

            return CreatedAtAction("GetGame", new { id = game.Id }, getGameDto);
        }

        // GET: api/Game/1/user
        [HttpGet("{id}/user")]
        [Authorize]
        public async Task<ActionResult<User>> GetGameUser(int id)
        {
            var game = await _gamesRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }
            
            var user = game.User;
            
            if (user == null)
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