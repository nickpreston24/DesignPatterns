using Shared;
using System.Collections.Generic;

namespace Movies.Data
{
    public interface IMovieRepository
    {
        IReadOnlyList<Movie> Find(Specification<Shared.Movie> specification);
    }
}
