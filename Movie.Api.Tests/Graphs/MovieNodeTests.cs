using DesignPatterns;
using Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Api;
using Movies.Api.Graphs;
using Movies.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Movies.Client;
using System.Text;
using System.Timers;

namespace Movie.Api.Tests.Graphs
{
    [TestClass]
    public class MovieNodeTests
    {
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

                //Assert.IsTrue(edgeCount > 0);
                Assert.IsTrue(nodeCount > 0);

                // Act
                RunIterator(movieGraph, bfsIterator);
            }
        }

        [TestMethod]
        public void CanStoreGraphToNeo4j()
        {
            // Todo: replace this with that query you found that
            // does a merge insert for new nodes
            string deleteAll = "MATCH (n:Movie) detach delete n";

            int port = 7687;
            using (var timer = TimeIt.GetTimer())
            using (IGraph graph = CreateMovieGraph(1000))
            using (var prototype = new HelloWorldExample($"bolt://localhost:{port}", "neo4j", "root"))
            {
                prototype.RunQuery(deleteAll);

                string query = MockNeo4JQueryBuilder<MovieNode>.MakeQuery(graph.Nodes.ToArray());
                Print(query);
                var results = prototype.RunQuery(query);
            }
        }

        private class MockNeo4JQueryBuilder<TNode>
        where TNode : INode
        {
            // Makes the query from ToString() of TNode.
            public static string MakeQuery(INode node)
            {
                return "CREATE (movie: Movie {" + node.ToString() + "})";
            }

            public static string MakeQuery(INode[] nodes)
            {
                int key = 0;
                var sb = nodes.Aggregate(new StringBuilder("CREATE \n"), (result, next)
                    => result.AppendLine("(movie" + key++
                        + ": Movie {" + next.ToString() + "}),"));
                sb.Length -= 3; //removes extra comma
                return sb.ToString();
            }

            //private static string MakeQueryFromProps(TNode node)
            //{
            //System.Reflection.PropertyInfo[] props = typeof(TNode).GetProperties();
            //string query = props.Aggregate(
            //    new StringBuilder("CREATE (movie:"), (sb, next) =>
            //    {
            //        //string value = next.GetValue(node).ToString();
            //        string value = node.ToString();
            //        sb.Append($"{value}");
            //        return sb;
            //    })
            //    .Append(")")
            //    .ToString();
            //}
        }

        [TestMethod]
        public void TestSpaceCasing()
        {
            // Todo: add a params delegate (see raindrop/history for ParamsFunc delegate that allowed multiple strings in a concat())
            var inputs = new string[] { "MovieGraph", "Movie Graph", "Movie-?#Graph", "-RAnDom -jUnk$__lol!" }.ToList();

            Console.WriteLine("cleaned:---");
            inputs.ForEach(t => Console.WriteLine(t.Clean()));

            Console.WriteLine("split---");
            inputs.ForEach(t => Console.WriteLine(t.SplitCamelCase()));

            Console.WriteLine("npm ---");
            inputs.ForEach(text => Console.WriteLine(
                Regex.Replace(text.Clean(), "/[W_]+(.|$)/g", "")));

            Print(inputs);
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

        private MovieGraph CreateMovieGraph(uint count = 10)
        {
            var movieLibrary = MockDb.GetMovies(count: count)
                .AsEnumerable()
                .Select(movie => movie.Map());

            var randomNodes =
                movieLibrary.Aggregate(new List<MovieNode>(), (result, movie) =>
                {
                    var node = new MovieNode(movie.ToDto())
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
            //actor.Relate(new WorksWith(actor2), actor, actor2);  //I'd like to use a delegate to hold & obfuscate the relationship
            actor.WorksWith(actor2); //Delegate to a the WorksWith<T> relationship

            Print(actor, actor2);
            return movieGraph;
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