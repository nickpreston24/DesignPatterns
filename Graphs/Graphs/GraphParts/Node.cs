using DesignPatterns;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public abstract class Node : INode
    {
        protected bool disposed = false; // To detect redundant calls

        protected IGraphBuilder<INode> graphBuilder = Singleton<GraphBuilder<INode>>.Instance;

        protected List<IRelationship> edges = new List<IRelationship>(0);

        public int Id { get; set; }

        protected Node() => graphBuilder.AddNode(this);

        public ICollection<IRelationship> Edges
        {
            get => edges;
            private set => edges.AddRange(value);
        }

        public INode Relate(IRelationship relationship, params INode[] neighbors)
        {
            //Todo: have the builder attach a relationship to this node and its neighbor(s).
            //graphBuilder.AddEdge(new Edge());
            return this;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Console.WriteLine($"Disposing { GetType().Name }...");
                    // TODO: dispose managed state (managed objects).

                    DisposeEdges();
                }

                // TODO: set large fields to null.
                disposed = true;
            }
        }

        private void DisposeEdges()
        {
            foreach (var edge in Edges ?? Enumerable.Empty<IEdge>())
            {
                edge.Dispose();
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}