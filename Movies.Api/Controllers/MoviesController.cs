using Microsoft.AspNetCore.Mvc;
using Movies.Shared;
using System.Collections.Generic;

namespace Movies.Api
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : Controller
    {
        private MoviesService service;

        public MoviesController(MoviesService moviesService) => service = moviesService;

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return new Movie[] { new Movie { Title = "The Matrix" },
                new Movie { Title = "Star Wars: A New Hope" } };
        }

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "The Dark Knight Rises";
        //}

        [HttpGet("{mpaa-rating}")]
        public IReadOnlyList<Movie> GetByRating(params string[] mpaaRatings)
        {
            var goodMpaaRating = new MpaaRating(mpaaRatings);
            var goodMovie = new GoodMovieSpecification(threshold: Shared.Movie.MAX_RATING - 2);

            var moviePick = goodMpaaRating.And(goodMovie);

            var movies = service
                .Find(moviePick)
                .ToDto();

            return movies;
        }

        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}