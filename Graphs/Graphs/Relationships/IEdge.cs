using System;

namespace Graphs
{
    /// <summary>
    /// Interface for a Neo4j-style relationship
    /// </summary>
    public interface IEdge : IDisposable
    {
        ///The starting Node from which an Edge projects relationships
        INode Subject { get; }

        ///The affected neighboring Node.
        INode Neighbor { get; set; }

        ///Determines if a given Node shares this Edge.
        bool IsEdgeOf(INode node);
    }

    /// <summary>
    /// Uni-directional Edge
    /// </summary>
    public interface IEdge<TSubject> : IEdge
        where TSubject : INode
    {
        /// Determines whether the given node is one of this edge's subjects.
        bool HasSubject(TSubject node);
    }

    /// <summary>
    /// Bi-directional Edge
    /// </summary>
    public interface IEdge<TSubject, TOther> : IEdge<TSubject>
        where TSubject : INode
        where TOther : INode
    {
        ///Determines if two nodes are neighbored via this Edge.
        bool AreNeighbors(TSubject node, TOther other);
    }
}