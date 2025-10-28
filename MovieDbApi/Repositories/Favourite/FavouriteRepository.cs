using Microsoft.EntityFrameworkCore;
using MovieApi.Models;
using MovieDbApi.Data;

namespace MovieApi.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly AppDbContext _context;

        public FavouriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Favourite?> GetByUserAndMovieAsync(int userId, int movieId)
        {
            return await _context.Favourites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);
        }

        public async Task<IEnumerable<Favourite>> GetByUserIdAsync(int userId)
        {
            return await _context.Favourites
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Favourite favourite)
        {
            await _context.Favourites.AddAsync(favourite);
        }

        public async Task DeleteAsync(Favourite favourite)
        {
            _context.Favourites.Remove(favourite);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
