using Microsoft.AspNetCore.Mvc;
using MovieDbApi.DTOs.Movie;
using MovieDbApi.Models;
using MovieDbApi.Repositories;

namespace MovieDbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllAsync();

            var result = movies.Select(m => new GetMovieDto
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                ImageUrl = m.ImageUrl,
                Language = m.Language,
                AudienceRating = m.AudienceRating
            });

            return Ok(result);
        }

        // GET: api/movies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
                return NotFound(new { Message = $"Movie with ID {id} not found." });

            var result = new GetMovieByIdDto
            {
                Title = movie.Title,
                StoryLine = movie.StoryLine,
                AudienceRating = movie.AudienceRating,
                AudienceCount = movie.AudienceCount,
                Genres = movie.Genres,
                ReleaseDate = movie.ReleaseDate,
                ImageUrl = movie.ImageUrl,
                RuntimeMinutes = movie.RuntimeMinutes,
                Language = movie.Language,
                Director = movie.Director,
                LeadActor = movie.LeadActor,
                LeadActress = movie.LeadActress,
                SupportingActors = movie.SupportingActors,
                Period = movie.Period
            };

            return Ok(result);
        }

        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] GetMovieByIdDto movieDto)
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
                RuntimeMinutes = movieDto.RuntimeMinutes,
                Language = movieDto.Language,
                Director = movieDto.Director,
                LeadActor = movieDto.LeadActor,
                LeadActress = movieDto.LeadActress,
                SupportingActors = movieDto.SupportingActors,
                Period = movieDto.Period
            };

            await _movieRepository.AddAsync(movie);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movieDto);
        }

        // PUT: api/movies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] GetMovieByIdDto movieDto)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
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
            existingMovie.RuntimeMinutes = movieDto.RuntimeMinutes;
            existingMovie.Language = movieDto.Language;
            existingMovie.Director = movieDto.Director;
            existingMovie.LeadActor = movieDto.LeadActor;
            existingMovie.LeadActress = movieDto.LeadActress;
            existingMovie.SupportingActors = movieDto.SupportingActors;
            existingMovie.Period = movieDto.Period;

            await _movieRepository.UpdateAsync(existingMovie);
            return NoContent();
        }

        // DELETE: api/movies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
                return NotFound(new { Message = $"Movie with ID {id} not found." });

            await _movieRepository.DeleteAsync(existingMovie);
            return NoContent();
        }

        // GET: api/movies/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than 0.");

            var (movies, totalCount) = await _movieRepository.GetPagedAsync(pageNumber, pageSize);

            var movieDtos = movies.Select(m => new GetMovieDto
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                ImageUrl = m.ImageUrl,
                Language = m.Language,
                AudienceRating = m.AudienceRating
            });

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Movies = movieDtos
            });
        }

        // GET: api/movies/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { Message = "Query parameter cannot be empty." });

            var (movies, totalCount) = await _movieRepository.SearchPagedAsync(query, pageNumber, pageSize);

            var movieDtos = movies.Select(m => new GetMovieDto
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                ImageUrl = m.ImageUrl,
                Language = m.Language,
                AudienceRating = m.AudienceRating
            });

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Movies = movieDtos
            });
        }

        // GET: api/movies/filter
        [HttpGet("filter")]
        public async Task<IActionResult> FilterMovies([FromQuery] string field, [FromQuery] string value, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(value))
                return BadRequest(new { Message = "Field and value parameters are required." });

            var (movies, totalCount) = await _movieRepository.FilterByFieldAsync(field, value, pageNumber, pageSize);

            var movieDtos = movies.Select(m => new GetMovieDto
            {
                Id = m.Id,
                Title = m.Title,
                ImageUrl = m.ImageUrl,
                ReleaseDate = m.ReleaseDate,
                Language = m.Language,
                AudienceRating = m.AudienceRating
            });

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new
            {
                Field = field,
                Value = value,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                Movies = movieDtos
            });
        }
    }
}