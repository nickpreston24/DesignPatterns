using Graphs;

namespace Movies.Api.Graphs
{
    /// <summary>
    /// A Derived iterable collection of Movie Nodes
    /// I've derived the iterator pattern
    /// </summary>
    public class MovieGraph : Graph
    {
        private bool disposedValue = false; // To detect redundant calls

        public int EdgeCount => GetEdges().Count;

        public int NodeCount => Nodes.Count;

        public MovieGraph(params MovieNode[] movieNodes)
            : base(movieNodes)
        {
        }

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //Console.WriteLine($"Disposing { GetType().Name }...");
                    foreach (var node in Nodes)
                        node.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                Nodes.Clear();
                //Nodes = null;

                disposedValue = true;
                //Console.WriteLine("Disposed MovieGraph...");
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MovieGraph()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}