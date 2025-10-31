using MovieApi.Models;

namespace MovieDbApi.Repositories
{
    public interface IFavouriteRepository
    {
        Task<Favourite?> GetByUserAndMovieAsync(int userId, int movieId);
        Task<IEnumerable<Favourite>> GetByUserIdAsync(int userId);
        Task AddAsync(Favourite favourite);
        Task DeleteAsync(Favourite favourite);
        Task SaveChangesAsync();
    }
}
