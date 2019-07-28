using DesignPatterns;
using Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Api.Graphs;
using Movies.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Movie.Api.Tests.Graphs
{
    [TestClass]
    public class MovieNodeTests
    {
        //MovieGraph movieGraph;

        //public MovieNodeTests()
        //{
        //    movieGraph = CreateMovieGraph();
        //}

        [TestMethod]
        public void CanIterateMovieGraph()
        {
            // Arrange
            using (MovieGraph movieGraph = CreateMovieGraph())
            using (var bfsIterator = new BreadthFirstIterator<MovieNode>(movieGraph))
            {
                // Assert Iterators instantiated:
                Assert.IsNotNull(bfsIterator);
                Assert.IsNotNull(movieGraph.Iterator());

                //Assert graph actually builds:
                int edgeCount = movieGraph.EdgeCount;
                int nodeCount = movieGraph.NodeCount;

                Print("Edges: " + edgeCount, "Nodes: " + nodeCount);
                Print("------------------------\n");

                Assert.IsTrue(edgeCount > 0);
                Assert.IsTrue(nodeCount > 0);

                // Act
                //RunIterator(movieGraph, bfsIterator);
            }
        }

        [TestMethod]
        public void TestSpaceCasing()
        {
            var inputs = new string[] { "MovieGraph", "Movie Graph", "Movie-?#Graph", "-RAnDom -jUnk$__lol!" }.ToList();

            Console.WriteLine("cleaned:---");
            inputs.ForEach(t => Console.WriteLine(t.Clean()));

            Console.WriteLine("split---");
            inputs.ForEach(t => Console.WriteLine(t.SplitCamelCase()));

            Console.WriteLine("npm ---");
            inputs.ForEach(text => Console.WriteLine(
                Regex.Replace(text.Clean(), "/[W_]+(.|$)/g", "")));
        }

        private bool RunIterator(MovieGraph movieGraph, IIterator<MovieNode> iterator)
        {
            int iterations = 0;

            while (iterator.HasNext)
            {
                INode nextNode = iterator.GetNext();
                iterations++;
                Print($"Node {iterations}: ", nextNode);
                Print(nextNode);
            }

            Print($"Iterations: {iterations} / {movieGraph.NodeCount}");
            Print($"% {iterations / movieGraph.NodeCount * 100M} Completed");

            return true;
        }

        private MovieGraph CreateMovieGraph()
        {
            var movieLibrary = MockDb.GetMovies()
                .AsEnumerable()
                .Select(movie => movie.Map());

            var randomNodes =
                movieLibrary.Aggregate(new List<MovieNode>(), (result, movie) =>
                {
                    var node = new MovieNode(new Movies.Api.Movie() { } /*movie.ToDto()*/)
                    {
                        Id = Enumerable.Range(1, movieLibrary.Count()).FirstRandom()
                    };
                    result.Add(node);

                    return result;
                })
                .ToArray();

            var movieGraph = new MovieGraph(randomNodes);

            MovieNode nodeA = randomNodes[0];
            MovieNode nodeB = randomNodes[1];

            Actor actor = new Actor { Name = "Robert Downey Jr." };
            Actor actor2 = new Actor { Name = "Mark Ruffalo" };

            //TODO: node.Relate(...graphbuilder.addEdge(n1,n2)..., IRelationship r)
            // TODO: may have to use Vistor here:
            //actor.Relate(new WorksWith(actor2), actor, actor2);
            //actor.WorksWith(actor2); //Delegate to a the WorksWith<T> relationship

            Print(actor, actor2);
            return movieGraph;
        }

        private void Print(params object[] collection)
        {
            foreach (var item in collection)
            {
                if (item is IEnumerable<object> list)
                    Print(list);

                Print(item);
            }
        }

        private void Print(object item)
        {
            if (item is IEnumerable<object> list)
                Print(new[] { list });

            Console.WriteLine(item?.ToString());
            Debug.WriteLine(item?.ToString());
        }
    }
}