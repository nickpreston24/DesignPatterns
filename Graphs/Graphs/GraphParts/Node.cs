using DesignPatterns;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public abstract class Node : INode
    {
        protected bool disposed = false; // To detect redundant calls

        private void DisposeEdges()
        {
            foreach (var edge in Edges ?? Enumerable.Empty<IEdge>())
            {
                edge.Dispose();
            }
        }

        protected IGraphBuilder<INode> graphBuilder = Singleton<GraphBuilder<INode>>.Instance;

        protected List<IRelationship> edges = new List<IRelationship>(0);

        protected Node() => graphBuilder.AddNode(this);

        public int Id { get; set; }

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
            if (disposed)
                return;

            if (disposing)
            {
                DisposeEdges();
            }

            disposed = true;
        }

        public void Dispose() => Dispose(true);
    }
}