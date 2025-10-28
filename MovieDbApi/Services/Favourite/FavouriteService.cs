using MovieApi.Models;
using MovieApi.Repositories;

namespace MovieApi.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _repository;

        public FavouriteService(IFavouriteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Favourite>> GetByUserIdAsync(int userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<bool> ToggleFavouriteAsync(int userId, int movieId)
        {
            var existing = await _repository.GetByUserAndMovieAsync(userId, movieId);
            if (existing != null)
            {
                await _repository.DeleteAsync(existing);
                await _repository.SaveChangesAsync();
                return false; // Removed
            }

            var fav = new Favourite { UserId = userId, MovieId = movieId };
            await _repository.AddAsync(fav);
            await _repository.SaveChangesAsync();
            return true; // Added
        }

        public async Task<bool> DeleteAsync(int userId, int movieId)
        {
            var existing = await _repository.GetByUserAndMovieAsync(userId, movieId);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
