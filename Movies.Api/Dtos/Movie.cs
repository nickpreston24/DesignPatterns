using Movies.Shared;

namespace Movies.Api
{
    public class Movie
    {
        public string Title { get; set; }

        public MPAARating MpaaRating { get; set; }
        public double Rating { get; set; }

        public override string ToString() =>
            $"{nameof(Title)}: '{Title}'," +
            $" {nameof(MpaaRating)}: '{MpaaRating}'," +
            $" {nameof(Rating)}: '{Rating}'";
    }
}