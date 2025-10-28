using MovieApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public interface IFavouriteService
    {
        Task<IEnumerable<Favourite>> GetByUserIdAsync(int userId);
        Task<bool> ToggleFavouriteAsync(int userId, int movieId);
        Task<bool> DeleteAsync(int userId, int movieId);
    }
}
