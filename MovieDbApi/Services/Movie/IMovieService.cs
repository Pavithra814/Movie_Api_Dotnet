using MovieDbApi.DTOs.Movie;
using MovieDbApi.Models;

public interface IMovieService
{
    Task<IEnumerable<MovieCardDto>> GetAllMoviesAsync();
    Task<MovieDisplayDto?> GetMovieByIdAsync(int id);
    Task<(IEnumerable<MovieCardDto> Movies, int TotalCount)> SearchPagedAsync(string query, int pageNumber, int pageSize);
    Task AddMovieAsync(Movie movie);
    Task UpdateMovieAsync(Movie movie);
    Task DeleteMovieAsync(Movie movie);
    Task<Movie?> GetMovieEntityByIdAsync(int id);
    Task<(IEnumerable<MovieCardDto> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<(IEnumerable<MovieCardDto> Movies, int TotalCount)> FilterByFieldAsync(string field, string value, int pageNumber, int pageSize);
}
