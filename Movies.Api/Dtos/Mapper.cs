using System.Collections.Generic;
using System.Linq;

namespace Movies.Api
{
    public static partial class Mapper
    {
        public static Movie ToDto(this Shared.Movie movie) => new Movie
        {
            Title = movie.Title,
            MpaaRating = movie.MpaaRating,
            Rating = movie.Rating
        };

        public static Shared.Movie Map(this Movie movie) => new Shared.Movie
        {
            Title = movie.Title,
            MpaaRating = movie.MpaaRating,
            Rating = movie.Rating
        };

        public static IReadOnlyList<Movie> ToDto(this IEnumerable<Shared.Movie> collection)
            => collection
                .Select(entity => entity.ToDto())
                .ToList();

        public static IReadOnlyList<Shared.Movie> Map(this IEnumerable<Movie> movies)
            => movies
                .Select(movie => movie.Map())
                .ToList();
    }
}