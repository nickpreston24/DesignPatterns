namespace Movies.Api
{
    // You can use Assemblers with Aggregates to create complext DTOs
    // with nested classes, collections, etc.
    public class MovieAssembler
    {
        public Movie Build(Shared.Movie movie) => movie.ToDto();
    }
}