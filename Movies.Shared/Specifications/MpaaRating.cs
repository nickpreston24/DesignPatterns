using DesignPatterns;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Movies.Shared
{
    public partial class MpaaRating : Specification<Movie>
    {
        // TODO: Add some currying here
        private Func<Movie, bool> nonAdult = movie
             => movie.MpaaRating != MPAARating.R
             && movie.MpaaRating != MPAARating.MA;

        public MPAARating[] Ratings { get; private set; }

        public MpaaRating(MPAARating mpaaRating)
            : this(new MPAARating[] { mpaaRating })
        {
        }

        public MpaaRating(params MPAARating[] ratings)
            => Ratings = ratings.Distinct().ToArray();

        public MpaaRating(params string[] mpaaRatings)
            : this(mpaaRatings.Select(rating => DetectRating(rating)).ToArray())
        {
        }

        public override Expression<Func<Movie, bool>> Condition()
            => movie
                => Ratings.Any(rating => movie.MpaaRating.Equals(rating))
                   && nonAdult(movie);

        private static MPAARating DetectRating(string rating)
        {
            Enum.TryParse(rating, true, out MPAARating selectedRating);
            return selectedRating;
        }
    }
}