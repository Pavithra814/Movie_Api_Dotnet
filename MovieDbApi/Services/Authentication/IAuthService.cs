using MovieDbApi.DTOs.User;
using MovieDbApi.Models;

namespace MovieDbApi.Services.Authentication
{
    public interface IAuthService
    {
        Task<User?> RegisterUserAsync(RegisterDto dto);
        Task<User?> LoginAsync(LoginDto dto);
        //Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);

        Task<User?> UpdateUserAsync(int id, UpdateUserDto dto);
 
    }
}
