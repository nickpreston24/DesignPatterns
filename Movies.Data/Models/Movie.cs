using Movies.Shared;
using System;

namespace Movies.Data
{
    public class Movie
    {
        public string Title { get; internal set; }

        public DateTime ReleaseDate { get; internal set; }

        public MPAARating MpaaRating { get; internal set; }

        public string Genre { get; internal set; }

        public static int MAX_RATING = 5;
        private double rating;

        public double Rating
        {
            get => rating;
            internal set => rating = value < MAX_RATING
                ? value
                : throw new ArgumentOutOfRangeException(nameof(Rating));
        }

        public override string ToString() => $"Title: {Title}\nMpaa: {MpaaRating}\nRating: {Rating}/{MAX_RATING} Release Date: {ReleaseDate}\n";
    }
}