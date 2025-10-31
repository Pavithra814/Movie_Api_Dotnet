using Microsoft.AspNetCore.Mvc;
using MovieDbApi.Services;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        //Get favourites by userId
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var favourites = await _favouriteService.GetByUserIdAsync(userId);
            return Ok(favourites);
        }

        //Toggle favourite (add/remove)
        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavourite([FromQuery] int userId, [FromQuery] int movieId)
        {
            var added = await _favouriteService.ToggleFavouriteAsync(userId, movieId);
            return Ok(new { success = true, added });
        }

        //  Explicit delete
        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromQuery] int userId, [FromQuery] int movieId)
        //{
        //    var success = await _favouriteService.DeleteAsync(userId, movieId);
        //    if (!success) return NotFound("Favourite not found");
        //    return Ok("Removed from favourites");
        //}
    }
}
