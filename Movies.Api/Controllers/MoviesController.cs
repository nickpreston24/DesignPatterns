using Microsoft.AspNetCore.Mvc;
using Movies.Shared;
using System;
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
           IEnumerable<Movie> Get(string mpaaRating)
        {
            Enum.TryParse("red", ignoreCase: true, out MpaaRating ratingSelected);
            var withRating = new MpaaRatingSpecification(ratingSelected);
            //bool isOk = rating.IsSatisfiedBy(movie);  //Exercising a single movie
            IReadOnlyList<Movie> movies = service.Find(withRating);
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
