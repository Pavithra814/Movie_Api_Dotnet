using MovieDbApi.Models;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int id);
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Movie movie);
    Task<(IEnumerable<Movie> Movies, int TotalCount)> SearchPagedAsync(string query, int pageNumber, int pageSize);
    Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<(IEnumerable<Movie> Movies, int TotalCount)> FilterByFieldAsync(string field, string value, int pageNumber, int pageSize);
}
