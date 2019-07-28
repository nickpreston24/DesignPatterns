using System;

namespace Graphs
{
    //Interface for a Neo4j-style relationship
    public interface IRelationship<TSubject> : IRelationship
        where TSubject : INode

    {
        bool AreRelated(TSubject node, TSubject neighbor);

        IRelationship Relate(TSubject node, TSubject neighbor);
    }

    public interface IRelationship<TSubject, Other> : IRelationship
        where TSubject : INode
        where Other : INode
    {
        bool AreRelated(TSubject node, Other other);

        IRelationship Relate(TSubject node, Other neighbor);
    }

    public interface IRelationship : IDisposable
    {
        //bool IsEdgeOf(INode subject);
    }
}