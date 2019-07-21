using Movies.Shared;

namespace Movies.Api
{
    public class Movie
    {
        public string Name { get; internal set; }
        public MpaaRating MpaaRating { get; internal set; }
    }
}