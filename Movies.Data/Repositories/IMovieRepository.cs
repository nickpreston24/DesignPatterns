using DesignPatterns;
using System.Collections.Generic;

namespace Movies.Data
{
    public interface IMovieRepository
    {
        IReadOnlyList<Movie> Find(Specification<Movie> specification);
    }
}