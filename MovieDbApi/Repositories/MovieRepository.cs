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

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies
                                 .Where(m => !m.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                                 .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie movie)
        {
            movie.IsDeleted = true;
            movie.UpdatedOn = DateOnly.FromDateTime(DateTime.UtcNow);
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> SearchPagedAsync(string query, int pageNumber, int pageSize)
        {
            var filteredQuery = _context.Movies
                .Where(m => !m.IsDeleted &&
                           (m.Title.Contains(query)));
                            //m.Genres.Contains(query) ||
                            //m.Language.Contains(query) ||
                            //m.Director.Contains(query) ||
                            //m.LeadActor.Contains(query) ||
                            //m.LeadActress.Contains(query) ||
                            //m.SupportingActors.Contains(query) ||
                            //m.Period.Contains(query)));

            var totalCount = await filteredQuery.CountAsync();
            var movies = await filteredQuery.Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Movies.Where(m => !m.IsDeleted);

            var totalCount = await query.CountAsync();
            var movies = await query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<(IEnumerable<Movie> Movies, int TotalCount)> FilterByFieldAsync(string field, string value, int pageNumber, int pageSize)
        {
            var query = _context.Movies.Where(m => !m.IsDeleted);

            switch (field.ToLower())
            {
                case "language":
                    query = query.Where(m => m.Language != null && m.Language.Contains(value));
                    break;
                case "director":
                    query = query.Where(m => m.Director != null && m.Director.Contains(value));
                    break;
                case "leadactor":
                    query = query.Where(m => m.LeadActor != null && m.LeadActor.Contains(value));
                    break;
                case "leadactress":
                    query = query.Where(m => m.LeadActress != null && m.LeadActress.Contains(value));
                    break;
                //case "audiencerating":
                //    query = query.Where(m => m.AudienceRating != null && (m.AudienceRating > value));
                //    break;
                case "supportingactors":
                    query = query.Where(m => m.SupportingActors != null && m.SupportingActors.Contains(value));
                    break;
                case "period":
                    query = query.Where(m => m.Period != null && m.Period.Contains(value));
                    break;
                case "runtime":
                    if (int.TryParse(value, out int runtime))
                        query = query.Where(m => m.RuntimeMinutes == runtime);
                    break;
                default:
                    throw new ArgumentException("Invalid search field");
            }

            var totalCount = await query.CountAsync();
            var movies = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (movies, totalCount);
        }
       }
}
