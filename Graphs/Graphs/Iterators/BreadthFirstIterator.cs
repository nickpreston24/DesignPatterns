using DesignPatterns;
using System;

namespace Graphs
{
    /// <summary>
    /// Breadth First Search Iterator Implementation
    /// </summary>
    /// <seealso cref="Movies.Api.Graphs.IIterator{Movies.Api.Graphs.MovieNode}" />
    public class BreadthFirstIterator<T> : IIterator<T>
        where T : INode
    {
        private IGraph graph;
        private IteratorAdapter<T> adapter;

        public bool HasNext => graph.Iterator().HasNext;

        public BreadthFirstIterator(IGraph graph)
        {
            this.graph = graph;
            adapter = new IteratorAdapter<T>(this);
        }

        // TODO: See if there's a BFS way of removing a node intelligently
        public void Remove() => throw new NotImplementedException();

        // TODO: Implement BFS's method of getting the next node
        public T GetNext()
        {
            return (T)graph.Iterator().GetNext();
        }

        public void Dispose() => graph.Dispose();
    }
}