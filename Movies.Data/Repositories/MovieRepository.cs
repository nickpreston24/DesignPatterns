using Movies.Shared;
using Shared;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Data
{
    /// <summary>
    /// TODO: Integrate your favorite fluent Neo4j library here!
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        public IReadOnlyList<Shared.Movie> Find(Specification<Movie> specification)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Query<Movie>()
                    .Where(specification.ToExpression())
                    .Map() as IReadOnlyList<Shared.Movie>;
            }
        }

        public IReadOnlyList<Movie> Find(Specification<Shared.Movie> specification) => throw new System.NotImplementedException();
    }
}