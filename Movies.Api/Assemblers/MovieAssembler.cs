namespace Movies.Api
{
    // You can use Assemblers with Aggregates to create complex DTOs
    // with nested classes, collections, etc.
    public class MovieAssembler
    {
        public Movie Build(Shared.Movie movie) => movie.ToDto();
    }
}