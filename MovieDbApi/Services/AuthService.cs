using MovieDbApi.DTOs.User;
using MovieDbApi.Models;
using MovieDbApi.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace MovieDbApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> RegisterUserAsync(RegisterDto dto)
        {
            if (await _userRepository.IsEmailExistsAsync(dto.Email))
                return null;

            CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsVerified = true, // developer mode OTP bypass
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<User?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null || !VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        //public async Task<IEnumerable<User>> GetAllUsersAsync()
        //{
        //    return await _userRepository.GetAllUsersAsync();
        //}

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            user.Name = dto.Name;
            user.Email = dto.Email;

            return await _userRepository.UpdateUserAsync(user);
        }

    }
}
