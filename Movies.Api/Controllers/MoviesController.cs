using Microsoft.AspNetCore.Mvc;
using Movies.Data;
using System.Collections.Generic;

namespace Movies.Api
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : Controller
    {
        private MoviesService service;

        public MoviesController(MoviesService moviesService) => service = moviesService;

        //[HttpGet]
        //public IEnumerable<Movie> Get()
        //{
        //    return new Movie[] { new Movie { Name = "The Matrix" },
        //        new Movie { Name = "Star Wars: A New Hope" } };
        //}

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "The Dark Knight Rises";
        //}

        [HttpGet("{mpaa-rating}")]
        public
           /*async Task<ActionResult<IEnumerable<Movie>>>*/
           //ActionResult<IEnumerable<Movie>>
           IReadOnlyList<Movie> GetByRating(params string[] mpaaRatings)
        {
            MpaaRatingSpecification withSpecification = new MpaaRatingSpecification(mpaaRatings);
            //.And(new Data.GoodMovieSpecification(threshold: Data.Movie.MAX_RATING - 2))
            ;
            IReadOnlyList<Movie> movies = service.Find(withSpecification).ToDto();
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