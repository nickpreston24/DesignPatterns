using Graphs;

namespace Movies.Api.Graphs
{
    //A Composite element
    public class MovieNode : Node
    {
        public Movie Movie { get; private set; }

        public MovieNode(Movie movie) => Movie = movie;

        public override string ToString() => $"Id: {Id} {Movie?.ToString()}";

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
    }
}