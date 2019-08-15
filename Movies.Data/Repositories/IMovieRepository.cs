using DesignPatterns;
using System.Collections.Generic;

namespace Movies.Data
{
    public interface IMovieRepository
    {
        IReadOnlyList<Shared.Movie> Find(Specification<Shared.Movie> specification);
    }
}