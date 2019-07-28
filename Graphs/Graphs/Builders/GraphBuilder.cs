using Shared;

namespace Graphs
{
    public class GraphBuilder<T> : IGraphBuilder<T>
        where T : INode
    {
        private T root;

        protected GraphBuilder()
        {
        }

        public GraphBuilder(T rootNode) => root = rootNode;

        public T Build() => root;

        public IGraphBuilder<T> AddEdge(IEdge edge)
        {
            root.Edges.Add(edge);
            return this;
        }

        public void AddNode(T node) => root = node;
    }
}