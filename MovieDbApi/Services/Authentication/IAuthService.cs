using MovieDbApi.DTOs.User;
using MovieDbApi.Models;

namespace MovieDbApi.Services.Authentication
{
    public interface IAuthService
    {
        Task<User?> RegisterUserAsync(RegisterDto dto);
        Task<User?> LoginAsync(LoginDto dto);
    }
}
