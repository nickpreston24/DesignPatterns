using System.Collections.Generic;
using System.Linq;

namespace Movies.Api
{
    public static partial class Mapper
    {
        public static Movie ToDto(this Shared.Movie movie) => new Movie
        {
            Name = movie.Name,
            MpaaRating = movie.MpaaRating,
        };

        public static IReadOnlyList<Movie> ToDto(this IEnumerable<Shared.Movie> collection) => collection
                .Select(entity => entity.ToDto())
                .ToList();
    }
}
