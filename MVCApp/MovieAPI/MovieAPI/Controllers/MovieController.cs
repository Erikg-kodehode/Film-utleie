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

        // ✅ Correct constructor to inject MovieRepository
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
    }
}
