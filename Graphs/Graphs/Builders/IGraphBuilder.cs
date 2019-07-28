using DesignPatterns;

namespace Graphs
{
    public interface IGraphBuilder<T> : ISingleton
        where T : INode
    {
        IGraphBuilder<T> AddEdge(IRelationship edge);

        T Build();

        void AddNode(T node);
    }
}