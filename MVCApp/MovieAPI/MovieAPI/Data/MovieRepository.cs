using MovieAPI.Models;
using System.Collections.Generic;
using System.Linq;

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
            var movies = _context.Movies.ToList();

            Console.WriteLine("\nDEBUG: Retrieved Movies from DB:");
            foreach (var movie in movies)
            {
                Console.WriteLine($"- ID: {movie.Id}, Title: {movie.Title}, Year: {movie.Year}, Genre: {movie.Genre}");
            }

            return movies;
        }

        public void AddMovie(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Title) && !string.IsNullOrWhiteSpace(movie.Genre))
            {
                _context.Movies.Add(movie);
                _context.SaveChanges(); // ✅ Ensure changes are saved
            }
        }

        public void UpdateMovie(Movie updatedMovie)
        {
            var existingMovie = _context.Movies.Find(updatedMovie.Id);
            if (existingMovie != null)
            {
                existingMovie.Title = updatedMovie.Title;
                existingMovie.Year = updatedMovie.Year;
                existingMovie.Genre = updatedMovie.Genre;
                _context.SaveChanges(); // ✅ Ensure updates are saved
            }
        }

        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges(); // ✅ Ensure deletion is saved
            }
        }
    }
}
