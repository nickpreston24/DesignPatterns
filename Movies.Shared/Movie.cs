using System;

namespace Movies.Shared
{
    public class Movie
    {
        public string Title { get; set; }
        public MpaaRating MpaaRating { get; set; }
        public double Rating { get; set; }
    }
}
