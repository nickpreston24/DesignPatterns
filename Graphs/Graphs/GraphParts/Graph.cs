using DesignPatterns;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    //Concrete collection
    public abstract class Graph : IGraph
    {
        protected readonly IIterator<INode> iterator;

        private List<INode> nodes = new List<INode>(0);

        public ICollection<INode> Nodes
        {
            get => nodes;
            set => nodes.AddRange(value);
        }

        protected Graph(params INode[] nodes)
        {
            Nodes = nodes;
            iterator = new EnumerableAdapter<INode>(Nodes).Iterator();
        }

        public virtual IIterator<INode> Iterator() => iterator;

        public abstract void Dispose();

        protected virtual ICollection<IEdge> GetEdges()
        {
            return (Nodes == null
                ? Enumerable.Empty<IEdge>()
                : Nodes.Where(n => n.Edges != null)
                    .SelectMany(n => n.Edges)
                )
                .ToList();
        }
    }
}