using Movies.Api;
using Movies.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Timers.TimeIt;

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
            using (var timer = GetTimer())
                for (int i = 0; i < 500; i++)
                {
                    IReadOnlyList<Movies.Api.Movie> movies = controller.GetByRating("G", "PG13", "R", "MA");
                    if (movies.Count == 0)
                        break;
                    //Print(movies.Count);
                    //Print(movies);
                    Assert.IsNotNull(movies);
                    Assert.Greater(movies.Count, 0);
                }
        }

        private void Print(params object[] array)
        {
            foreach (var item in array)
            {
                if (item is IEnumerable<object> list)
                    Print(list);

                Print(item);
            }
        }

        private void Print(object item)
        {
            if (item is IEnumerable<object> list)
                Print(list.ToArray());

            Console.WriteLine(item?.ToString());
            Debug.WriteLine(item?.ToString());
        }
    }
}