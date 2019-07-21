namespace Movies.Api
{
    // Use the Assembler as a builder of a complex DTO.
    public class MovieAssembler
    {
        public Movie Build(Shared.Movie movie) => movie.ToDto();
    }
}