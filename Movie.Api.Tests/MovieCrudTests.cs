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
            moviesService = new MoviesService(new MovieRepository(), new MovieAssembler());
            controller = new MoviesController(moviesService);
        }

        [Test]
        public void CanGetMoviesBySpecification()
        {
            //TODO: There is an issue where compiling multiple expressions, one dealing with a
            // collection produces a strange bug.  Look into it.
            IReadOnlyList<Movies.Api.Movie> results = controller.GetByRating("G", "PG13", "R");

            Print(results.Count);
            Print(results);
            Assert.IsNotNull(results);
            Assert.Greater(results.Count, 0);
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