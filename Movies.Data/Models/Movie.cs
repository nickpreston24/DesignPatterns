using Movies.Shared;
using System;

namespace Movies.Data
{
    public class Movie
    {
        public string Name { get; }

        public DateTime ReleaseDate { get; }

        public MpaaRating MpaaRating { get; }

        public string Genre { get; }

        public double Rating { get; }
    }
}