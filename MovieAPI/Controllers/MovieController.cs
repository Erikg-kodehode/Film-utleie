using Microsoft.AspNetCore.Mvc;
using MovieAPI.Data;
using MovieAPI.Models;
using System.Collections.Generic;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository _movieRepo;

        public MovieController(MovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        [HttpGet]
        public ActionResult<List<Movie>> GetMovies()
        {
            return Ok(_movieRepo.GetMovies());
        }

        [HttpPost]
        public ActionResult AddMovie([FromBody] Movie movie)
        {
            _movieRepo.AddMovie(movie);
            return Ok("Movie added successfully!");
        }

        // ✅ Ensure this method is correctly formatted
        [HttpPut("{id}")]
        public ActionResult UpdateMovie(int id, [FromBody] Movie updatedMovie)
        {
            var movie = _movieRepo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            movie.Title = updatedMovie.Title;
            movie.Year = updatedMovie.Year;
            movie.Genre = updatedMovie.Genre;
            _movieRepo.UpdateMovie(movie);

            return Ok("Movie updated successfully!");
        }

        // ✅ Ensure this method is correctly formatted
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            var movie = _movieRepo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            _movieRepo.DeleteMovie(id);
            return Ok("Movie deleted successfully!");
        }
    }
}
