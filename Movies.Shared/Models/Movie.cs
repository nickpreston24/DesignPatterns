﻿using System;

namespace Movies.Shared
{
    public class Movie
    {
        public static int MAX_RATING { get; set; }
        public string Title { get; set; }
        public MpaaRating MpaaRating { get; set; }
        public double Rating { get; set; }
    }
}