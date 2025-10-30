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

        // ✅ GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new { user.Id, user.Name, user.Email });
        }

        // ✅ PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var updatedUser = await _authService.UpdateUserAsync(id, dto);
            if (updatedUser == null)
                return NotFound("User not found.");

            return Ok(new { updatedUser.Id, updatedUser.Name, updatedUser.Email });
        }
    }
}
