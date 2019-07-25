using Shared;
using System;
using System.Linq.Expressions;

namespace Movies.Data
{
    public class GoodMovie : Specification<Movie>
    {
        public int Threshold { get; set; }
        public GoodMovie(int threshold) => Threshold = threshold;

        public override Expression<Func<Movie, bool>> ToExpression() => movie => movie.Rating >= Threshold;
    }
}