using MovieDbApi.DTOs.Movie;
using MovieDbApi.Models;
using MovieDbApi.Repositories;

namespace MovieDbApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<MovieCardDto>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllAsync();
            return movies.Select(m => new MovieCardDto
            {
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                ImageUrl = m.ImageUrl
            });
        }

        public async Task<MovieDisplayDto?> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) return null;

            return new MovieDisplayDto
            {
                Title = movie.Title,
                StoryLine = movie.StoryLine,
                AudienceRating = movie.AudienceRating,
                AudienceCount = movie.AudienceCount,
                Genres = movie.Genres,
                ReleaseDate = movie.ReleaseDate
            };
        }

        public async Task<(IEnumerable<MovieCardDto> Movies, int TotalCount)> SearchPagedAsync(string query, int pageNumber, int pageSize)
        {
            var (movies, totalCount) = await _movieRepository.SearchPagedAsync(query, pageNumber, pageSize);

            var movieDtos = movies.Select(m => new MovieCardDto
            {
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                ImageUrl = m.ImageUrl
            });

            return (movieDtos, totalCount);
        }


        public async Task AddMovieAsync(Movie movie)
        {
            await _movieRepository.AddAsync(movie);
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            await _movieRepository.UpdateAsync(movie);
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            await _movieRepository.DeleteAsync(movie);
        }

        public async Task<Movie?> GetMovieEntityByIdAsync(int id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task<(IEnumerable<MovieCardDto> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var (movies, totalCount) = await _movieRepository.GetPagedAsync(pageNumber, pageSize);

            //Map Movie → MovieCardDto
            var movieDtos = movies.Select(m => new MovieCardDto
            {
                Title = m.Title,
                ReleaseDate = m.ReleaseDate, // adjust format as needed
                ImageUrl = m.ImageUrl
            });

            return (movieDtos, totalCount);
        }

    }
}
