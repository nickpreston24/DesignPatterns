using DesignPatterns;
using Movies.Data;
using System.Collections.Generic;

namespace Movies.Api
{
    public class MoviesService
    {
        private readonly IMovieRepository movies;
        private readonly MovieAssembler assembler;

        public MoviesService(IMovieRepository movieRepository)
        {
            movies = movieRepository;
            assembler = new MovieAssembler();
        }

        internal IReadOnlyList<Shared.Movie> Find(ISpecification<Shared.Movie> specification)
        {
            IReadOnlyList<Shared.Movie> movies = this.movies.Find(specification);
            return movies;
        }
    }
}