using System.Security.Claims;
using AutoMapper;
using backend.DTOs.Game;
using backend.DTOs.User;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IGamesRepository _gamesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IGameService _gamesService;
    private readonly ISetValidationService _setValidationService;
    private readonly IMapper _mapper;

    public GamesController(IGamesRepository gamesRepository, IUsersRepository usersRepository, IGameService gameService, ISetValidationService setValidationService, IMapper mapper)
    {
        _gamesRepository = gamesRepository;
        _usersRepository = usersRepository;
        _gamesService = gameService;
        _setValidationService = setValidationService;
        _mapper = mapper;
    }

    // GET: api/Games/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetGameDto>> GetGame(int id)
    {
        var game = await _gamesRepository.GetGameWithStatesAndFoundSetsAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        Console.WriteLine("---------------------------------------------------");

        var tableCards = game.GameCardStates
            .Where(gs => gs.Location == CardLocation.Table)
            .Select(gs => gs.Card)
            .ToList();

        Console.WriteLine(tableCards);

        int possibleSetsCount = _setValidationService.FindAllSets(tableCards).Count;

        Console.WriteLine(possibleSetsCount);

        var getGameDto = _mapper.Map<GetGameDto>(game);
        getGameDto.PossibleSetsCount = possibleSetsCount;

        return getGameDto;
    }

    // POST: api/Games
    [HttpPost]
    // [Authorize]
    public async Task<ActionResult<GetGameDto>> PostGame()
    {
        // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = "483811d3-be1a-4c55-b69e-e7d03414e8b5";

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

    // GET: api/Games
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetBaseGameDto>>> GetUserGames()
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

        var games = await _gamesRepository.GetGamesByUserAsync(userId);

        return Ok(_mapper.Map<IEnumerable<GetBaseGameDto>>(games));
    }


    // GET: api/Games/1/user
    [HttpGet("{id}/user")]
    [Authorize]
    public async Task<ActionResult<GetUserDto>> GetGameUser(int id)
    {
        var game = await _gamesRepository.GetGameWithUserAsync(id);

        if (game == null)
        {
            return NotFound("Game not found.");
        }

        var user = game.User;

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(_mapper.Map<GetUserDto>(user));
    }

    // POST: /api/Games/1/checkSet
    [HttpPost("{id}/checkset")]
    [Authorize]
    public async Task<ActionResult<CheckSetResponseDto>> CheckSet(int id, [FromBody] CheckSetDto checkSetDto)
    {
        var game = await _gamesRepository.GetGameWithStatesAsync(id);

        Console.WriteLine($"Game ID: {checkSetDto.Card1Id}, {checkSetDto.Card2Id}, {checkSetDto.Card3Id}");

        if (game == null)
        {
            return NotFound();
        }

        // check that gamne belongs to user (from token)
        if (game.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return Forbid();
        }


        var isValidSet = await _gamesService.CheckSet(id, checkSetDto);

        return Ok(isValidSet);
    }

    private async Task<bool> GameExists(int id)
    {
        return await _gamesRepository.Exists(id);
    }
}