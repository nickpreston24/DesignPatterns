using Movies.Data;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Api
{
    public class MoviesService
    {
        private readonly IMovieRepository movies;
        private readonly MovieAssembler assembler;
        public MoviesService(IMovieRepository movieRepository, MovieAssembler assembler)
        {
            movies = movieRepository;
            this.assembler = assembler;
        }

        internal IReadOnlyList<Api.Movie> Find(Specification<Data.Movie> specification)
        {
            IList<Shared.Movie> movies = this.movies.Find(specification).Map();
            return movies.Select(movie => assembler.Build(movie)).ToList();
        }
    }
}
