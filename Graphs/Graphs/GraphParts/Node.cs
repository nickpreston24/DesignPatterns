using DesignPatterns;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public abstract class Node : INode
    {
        protected bool disposed = false; // To detect redundant calls
        protected List<IEdge> edges = new List<IEdge>(0);
        protected IGraphBuilder<INode> graphBuilder = Singleton<GraphBuilder<INode>>.Instance;

        protected Node() => graphBuilder.AddNode(this);

        public ICollection<IEdge> Relationships
        {
            get => edges;
            private set => edges.AddRange(value);
        }

        public int Id { get; set; }

        public bool IsRelatedTo(INode other) => throw new System.NotImplementedException();

        //public INode Relate(IEdge relationship, params INode[] neighbors)
        //{
        //    //Todo: have the builder attach a relationship to this node and its neighbor(s).
        //    //graphBuilder.AddEdge(new Edge());
        //    return this;
        //}

        #region IDisposable Implementation

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DisposeEdges();
            }

            disposed = true;
        }

        private void DisposeEdges()
        {
            foreach (var edge in Relationships ?? Enumerable.Empty<IEdge>())
            {
                edge.Dispose();
            }
        }

        #endregion IDisposable Implementation
    }
}