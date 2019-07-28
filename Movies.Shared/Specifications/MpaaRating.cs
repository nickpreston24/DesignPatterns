using DesignPatterns;
using Movies.Shared;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Movies.Data
{
    public class MpaaRatingSpecification : Specification<Movie>
    {
        // TODO: Add some currying here
        private Func<Movie, bool> nonAdult = movie
             => movie.MpaaRating != MpaaRating.R
             && movie.MpaaRating != MpaaRating.MA;

        public MpaaRating[] Ratings { get; private set; }

        public MpaaRatingSpecification(MpaaRating mpaaRating)
            : this(new MpaaRating[] { mpaaRating })
        {
        }

        public MpaaRatingSpecification(params MpaaRating[] ratings)
            => Ratings = ratings.Distinct().ToArray();

        public MpaaRatingSpecification(params string[] mpaaRatings)
            : this(mpaaRatings.Select(rating => DetectRating(rating)).ToArray())
        {
        }

        public override Expression<Func<Movie, bool>> ToExpression()
            => movie
                => Ratings.Any(rating => movie.MpaaRating.Equals(rating))
                   && nonAdult(movie);

        private static MpaaRating DetectRating(string rating)
        {
            Enum.TryParse(rating, ignoreCase: true, out MpaaRating selectedRating);
            return selectedRating;
        }
    }
}