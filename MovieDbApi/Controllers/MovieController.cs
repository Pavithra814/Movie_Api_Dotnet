using Microsoft.AspNetCore.Mvc;
using MovieDbApi.DTOs.Movie;
using MovieDbApi.Models;
using MovieDbApi.Services;

namespace MovieDbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        // GET: api/movies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(new { Message = $"Movie with ID {id} not found." });

            return Ok(movie);
        }

        // GET: api/movies/search?query=action&pageNumber=1&pageSize=10
        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { Message = "Query parameter cannot be empty." });

            var (movies, totalCount) = await _movieService.SearchPagedAsync(query, pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Movies = movies
            };

            return Ok(response);
        }


        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieDisplayDto movieDto)
        {
            if (movieDto == null)
                return BadRequest(new { Message = "Movie data is required." });

            var movie = new Movie
            {
                Title = movieDto.Title,
                StoryLine = movieDto.StoryLine,
                AudienceRating = movieDto.AudienceRating,
                AudienceCount = movieDto.AudienceCount,
                Genres = movieDto.Genres,
                ReleaseDate = movieDto.ReleaseDate,
                ImageUrl = movieDto.ImageUrl,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,

                // New fields
                RuntimeMinutes = movieDto.RuntimeMinutes,
                Language = movieDto.Language,
                Director = movieDto.Director,
                LeadActor = movieDto.LeadActor,
                LeadActress = movieDto.LeadActress,
                SupportingActors = movieDto.SupportingActors,
                Period = movieDto.Period
            };

            await _movieService.AddMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movieDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDisplayDto movieDto)
        {
            var existingMovie = await _movieService.GetMovieEntityByIdAsync(id);
            if (existingMovie == null)
                return NotFound(new { Message = $"Movie with ID {id} not found." });

            existingMovie.Title = movieDto.Title;
            existingMovie.StoryLine = movieDto.StoryLine;
            existingMovie.AudienceRating = movieDto.AudienceRating;
            existingMovie.AudienceCount = movieDto.AudienceCount;
            existingMovie.Genres = movieDto.Genres;
            existingMovie.ReleaseDate = movieDto.ReleaseDate;
            existingMovie.ImageUrl = movieDto.ImageUrl;
            existingMovie.UpdatedOn = DateOnly.FromDateTime(DateTime.UtcNow);

            // New fields
            existingMovie.RuntimeMinutes = movieDto.RuntimeMinutes;
            existingMovie.Language = movieDto.Language;
            existingMovie.Director = movieDto.Director;
            existingMovie.LeadActor = movieDto.LeadActor;
            existingMovie.LeadActress = movieDto.LeadActress;
            existingMovie.SupportingActors = movieDto.SupportingActors;
            existingMovie.Period = movieDto.Period;

            await _movieService.UpdateMovieAsync(existingMovie);
            return NoContent();
        }
        // DELETE: api/movies/{id}  (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var existingMovie = await _movieService.GetMovieEntityByIdAsync(id); // Add helper in service
            if (existingMovie == null)
                return NotFound(new { Message = $"Movie with ID {id} not found." });

            await _movieService.DeleteMovieAsync(existingMovie); // Soft delete
            return NoContent();
        }

        //For Pagination
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than 0.");

            var (movies, totalCount) = await _movieService.GetPagedAsync(pageNumber, pageSize);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Movies = movies
            };

            return Ok(response);
        }

        // GET: api/movies/filter?field=director&value=Mani%20Ratnam&pageNumber=1&pageSize=10
        [HttpGet("filter")]
        public async Task<IActionResult> FilterMovies(
            [FromQuery] string field,
            [FromQuery] string value,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(value))
                return BadRequest(new { Message = "Field and value parameters are required." });

            var (movies, totalCount) = await _movieService.FilterByFieldAsync(field, value, pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var response = new
            {
                Field = field,
                Value = value,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Movies = movies
            };

            return Ok(response);
        }


    }
}
