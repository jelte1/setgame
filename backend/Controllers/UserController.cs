using backend.Interfaces;
using backend.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IUsersRepository _usersRepository;

        public UserController(IAuthManager authManager, IUsersRepository usersRepository)
        {
            this._authManager = authManager;
            this._usersRepository = usersRepository;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterLoginUserDto registerUserDto)
        {
            var errors = await _authManager.Register(registerUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterLoginUserDto loginUserDto)
        {
            var authResponse = await _authManager.Login(loginUserDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
        
        // POST: api/User/refreshtoken
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDto requestDto)
        {
            var authResponse = await _authManager.VerifyRefreshToken(requestDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
        
        // GET: api/User/1/Games
        [HttpGet("{id}/Games")]
        public async Task<IActionResult> GetUserGames(string id)
        {
            var games = await _usersRepository.GetUserGames(id);
            return Ok(games);
        }
    }
}