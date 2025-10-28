using Microsoft.AspNetCore.Mvc;
using MovieDbApi.DTOs.User;
using MovieDbApi.Services.Authentication;

namespace MovieDbApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = await _authService.RegisterUserAsync(dto);
            if (user == null)
                return BadRequest("Email already exists.");

            return Ok(new { user.Id, user.Name, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _authService.LoginAsync(dto);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            return Ok(new { user.Id, user.Name, user.Email });
        }
    }
}
