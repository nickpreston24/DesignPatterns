using Movies.Shared;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Data
{
    /// <summary>
    /// TODO: Integrate your favorite fluent Neo4j library here!
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        public IReadOnlyList<Movie> Find(Specification<Movie> specification)
        {
            // BTW: Session will be something like an EF Context, but for Neo4j
            using (ISession session = SessionFactory.OpenSession())
            {
                return /*session.Query<Movie>()*/
                    MockDb.GetMovies() //Temporary
                    .Where(specification.ToExpression())
                    .ToList();
            }
        }

        private class MockDb
        {
            private static readonly char[] alphabet = Enumerable.Range('A', 26)
                .Select(n => (char)n).ToArray();

            internal static IQueryable<Movie> GetMovies() =>
                Enumerable.Range(1, 100)
                .Aggregate(new List<Movie>(), (result, next) =>
                {
                    result.Add(new Movie
                    {
                        MpaaRating = GetMpaaRating(),
                        Name = GetTitle(),
                        Rating = GetRating(),
                    });
                    return result;
                })
                .AsQueryable();

            private static int GetRating() => Enumerable.Range(1, Movie.MAX_RATING - 1).FirstRandom();

            private static MpaaRating GetMpaaRating() => EnumExtensions.GetRandom<MpaaRating>();

            private static string GetTitle() => alphabet.TakeRandom(10)
                .Aggregate(new StringBuilder(), (res, nxt) =>
                {
                    res.Append(nxt);
                    return res;
                }).ToString();
        }
    }
}