using DesignPatterns;
using System;
using System.Linq.Expressions;

namespace Movies.Shared
{
    public class GoodMovieSpecification : Specification<Movie>
    {
        public int Threshold { get; set; }

        public GoodMovieSpecification(int threshold)
            => Threshold = threshold;

        public override Expression<Func<Movie, bool>> Condition()
            => movie => movie.Rating >= Threshold;
    }
}