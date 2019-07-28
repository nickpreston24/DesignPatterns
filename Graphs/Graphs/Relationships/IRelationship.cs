using System;

namespace Graphs
{
    //Interface for a Neo4j-style relationship
    public interface IRelationship<TSubject> : IRelationship
        where TSubject : INode
    {
        TSubject Subject { get; }

        //bool AreRelated(TSubject node, TSubject neighbor);

        //IRelationship Relate(TSubject node, TSubject neighbor);
    }

    public interface IRelationship<TSubject, Other> : IRelationship
        where TSubject : INode
        where Other : INode
    {
        TSubject Subject { get; }
        Other Neighbor { get; }

        //bool AreRelated(TSubject node, Other other);

        //IRelationship Relate(TSubject node, Other neighbor);
    }

    public interface IRelationship : IEdge
    {
        //bool IsEdgeOf(INode subject);
    }
}