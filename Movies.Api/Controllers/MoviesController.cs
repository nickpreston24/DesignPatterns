using Microsoft.AspNetCore.Mvc;
using Movies.Data;
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
            var mpaaSpec = new MpaaRatingSpecification(mpaaRatings);
            var goodSpec = new GoodMovieSpecification(threshold: Shared.Movie.MAX_RATING - 2);

            var compoundSpecification = new MpaaRatingSpecification(mpaaRatings)
                .And(new GoodMovieSpecification(threshold: Data.Movie.MAX_RATING - 2));

            //var movies = service.Find(compoundSpecification).ToDto();
            //var movies = service.Find(mpaaSpec).ToDto();
            var movies = service.Find(mpaaSpec.And(goodSpec)).ToDto();
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