using System.Collections.Generic;
using System.Linq;

namespace Movies.Data
{
    public static partial class Mapper
    {
        /// <summary>
        /// Maps the specified collection.
        /// 
        /// By default, I'll map to IList when coming back from a repository.
        /// ToList() for implicitly runs any IQueryable without having to do so within repository.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IList<Shared.Movie> Map(this IEnumerable<Movie> collection) => collection
                .Select(entity => entity.Map())
                .ToList();

        public static Shared.Movie Map(this Movie entity) => new Shared.Movie
        {
            Title = entity.Title,
            MpaaRating = entity.MpaaRating,
            Rating = entity.Rating
        };
    }
}
