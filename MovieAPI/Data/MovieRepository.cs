using System.Collections.Generic;
using System.Linq;
using MovieAPI.Models;

namespace MovieAPI.Data
{
    public class MovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetMovies()
        {
            return _context.Movies.ToList();
        }

        public void AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        // ✅ Add this method to fix the error
        public Movie? GetMovieById(int id)
        {
            return _context.Movies.Find(id);
        }

        public void UpdateMovie(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
        }

        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
    }
}
