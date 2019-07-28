using DesignPatterns;
using System.Collections.Generic;
using System.Linq;

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
    }
}