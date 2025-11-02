using Microsoft.AspNetCore.Mvc;
using MovieDbApi.Repositories;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteController(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }

        // GET: api/favourite/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var favourites = await _favouriteRepository.GetByUserIdAsync(userId);

            // Project into a clean shape (avoid circular JSON references)
            var result = favourites.Select(f => new
            {
                f.Id,
                f.UserId,
                f.MovieId,
                Movie = new
                {
                    f.Movie.Id,
                    f.Movie.Title,
                    f.Movie.ImageUrl,
                    f.Movie.ReleaseDate,
                    f.Movie.Genres,
                    f.Movie.StoryLine,
                    f.Movie.AudienceRating,
                    f.Movie.AudienceCount,
                    f.Movie.Language,
                    f.Movie.Director,
                    f.Movie.LeadActor,
                    f.Movie.LeadActress,
                    f.Movie.SupportingActors,
                    f.Movie.RuntimeMinutes,
                    f.Movie.Period
                }
            });

            return Ok(result);
        }


        // POST: api/favourite/toggle?userId=1&movieId=5
        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavourite([FromQuery] int userId, [FromQuery] int movieId)
        {
            var existing = await _favouriteRepository.GetByUserAndMovieAsync(userId, movieId);

            if (existing != null)
            {
                await _favouriteRepository.DeleteAsync(existing);
                await _favouriteRepository.SaveChangesAsync();
                return Ok(new { success = true, added = false });
            }

            var favourite = new Favourite { UserId = userId, MovieId = movieId };
            await _favouriteRepository.AddAsync(favourite);
            await _favouriteRepository.SaveChangesAsync();
            return Ok(new { success = true, added = true });
        }

        // Optional: DELETE api/favourite?userId=1&movieId=5
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int userId, [FromQuery] int movieId)
        {
            var existing = await _favouriteRepository.GetByUserAndMovieAsync(userId, movieId);
            if (existing == null)
                return NotFound("Favourite not found");

            await _favouriteRepository.DeleteAsync(existing);
            await _favouriteRepository.SaveChangesAsync();
            return Ok("Removed from favourites");
        }
    }
}
