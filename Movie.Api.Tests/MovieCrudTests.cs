using Movies.Api;
using Movies.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tests
{
    public class Tests
    {
        private MoviesService moviesService;
        private MoviesController controller;

        [SetUp]
        public void Setup()
        {
            moviesService = new MoviesService(new MovieRepository());
            controller = new MoviesController(moviesService);
        }

        [Test]
        public void CanGetMoviesBySpecification()
        {
            //TODO: There is an issue where compiling multiple expressions, one dealing with a
            // collection produces a strange bug.  Look into it.

            for (int i = 0; i < 5; i++)
            {
                IReadOnlyList<Movies.Api.Movie> movies = controller.GetByRating("G", "PG13", "R");
                Print(movies.Count);
                //Print(movies);
                Assert.IsNotNull(movies);
                Assert.Greater(movies.Count, 0);
            }
        }

        private void Print(IEnumerable<object> collection)
        {
            foreach (var item in collection)
            {
                Print(item);
            }
        }

        private void Print(object value)
        {
            Console.WriteLine(value?.ToString());
            Debug.WriteLine(value?.ToString());
        }
    }
}