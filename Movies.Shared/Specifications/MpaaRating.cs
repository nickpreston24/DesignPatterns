using System;
using System.Linq.Expressions;
using Shared;

namespace Movies.Shared
{
    public class MpaaRatingSpecification : Specification<Movie>
    {
        public MpaaRating Rating { get; private set; }
        public MpaaRatingSpecification(MpaaRating mpaaRating) => Rating = mpaaRating;

        public override Expression<Func<Movie, bool>> ToExpression() => movie => movie.MpaaRating <= Rating;
    }
}
