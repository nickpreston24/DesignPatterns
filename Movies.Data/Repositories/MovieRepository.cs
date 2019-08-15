using DesignPatterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Movies.Data
{
    /// <summary>
    /// TODO: Integrate your favorite fluent Neo4j library here!
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private Func<Shared.Movie, bool> condition;

        public IReadOnlyList<Shared.Movie> Find(Specification<Shared.Movie> specification)
        {
            condition = condition ?? specification.Condition().Compile();
            //var recompiledExpr = specification.Condition().Compile().ToExpression();
            // BTW: Session will be something like an EF Context, but for Neo4j
            //TODO: find a way to convert a Specification to one that uses Data.Movie and can be used in IQueryable filters.
            using (var timer = TimeIt.GetTimer())
            using (ISession session = SessionFactory.OpenSession())
            {
                return MockDb.GetMovies()
                        .Map()
                        .Where(condition)
                        .ToList();
            }
        }
    }
}