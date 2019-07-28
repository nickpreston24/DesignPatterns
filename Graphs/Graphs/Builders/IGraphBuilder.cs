using DesignPatterns;
using Graphs;

namespace Graphs
{
    public interface IGraphBuilder<T> : ISingleton
        where T : INode
    {
        IGraphBuilder<T> AddEdge(IEdge edge);

        T Build();

        void AddNode(T node);
    }
}