using MovieDbApi.Models;

namespace MovieDbApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> SoftDeleteUserAsync(int id);
        Task<bool> IsEmailExistsAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> UpdateUserAsync(User user);
 
    }
}
