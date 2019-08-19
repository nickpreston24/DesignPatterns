using Graphs;

namespace Movies.Api.Graphs
{
    public class MovieNode : Node
    {
        public MovieNode(Movie movie) => Movie = movie;

        public Movie Movie { get; private set; }

        public static implicit operator MovieNode(Movie movie) => new MovieNode(movie);

        public static implicit operator Movie(MovieNode node) => node.Movie;

        public override string ToString() => $"{nameof(Id)}: '{Id}', {Movie?.ToString()}";

        #region IDisposable Implementation

        public new void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    base.Dispose();
                }

                Movie = null;
                disposed = false;
            }
        }

        public new void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion IDisposable Implementation
    }
}