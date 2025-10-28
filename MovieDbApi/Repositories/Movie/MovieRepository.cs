using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieDbApi.Data;
using MovieDbApi.Models;

namespace MovieDbApi.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieDbApi.Models.Movie>> GetAllAsync()
        {
            return await _context.Movies
                                 .Where(m => !m.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<MovieDbApi.Models.Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                                 .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task AddAsync(MovieDbApi.Models.Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MovieDbApi.Models.Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MovieDbApi.Models.Movie movie)
        {
            // Soft delete
            movie.IsDeleted = true;
            movie.UpdatedOn = DateOnly.FromDateTime(DateTime.UtcNow);
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<MovieDbApi.Models.Movie> Movies, int TotalCount)> SearchPagedAsync(string query, int pageNumber, int pageSize)
        {
            var filteredQuery = _context.Movies
                                        .Where(m => !m.IsDeleted &&
                                                   (m.Title.Contains(query) || m.Genres.Contains(query)));

            var totalCount = await filteredQuery.CountAsync();

            var movies = await filteredQuery
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<(IEnumerable<MovieDbApi.Models.Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Movies
                                .Where(m => !m.IsDeleted);

            var totalCount = await query.CountAsync();

            var movies = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return (movies, totalCount);
        }
    }
}
