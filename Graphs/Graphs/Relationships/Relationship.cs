using DesignPatterns;
using System;
using System.Linq.Expressions;

namespace Graphs
{
    /// <summary>
    /// A Relationship should:
    /// Tell if it is an edge on a given Node.
    /// Chain to another specification
    /// Compile its expression(s)
    /// </summary>
    /// <typeparam name="TNode">The type of the node.</typeparam>
    /// <seealso cref="Movies.Api.Graphs.IRelationship" />
    ///

    public abstract class Relationship<TNode> : Specification<TNode>, IRelationship
        where TNode : INode
    {
        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Relationship()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }

    //public abstract class Relationship<TNode> : IRelationship<TNode>
    //    where TNode : INode
    //{
    //    public abstract Expression<Func<TNode, TNode, bool>> Condition();

    //    public bool Relate(TNode node, TNode neighbor)
    //    {
    //        var predicate = Condition().Compile();
    //        return predicate(node, neighbor);
    //    }

    //    // Relate a given node to his neighbor by establishing a relationship between the two
    //    // Add this relationship to both nodes' edges (collection of relationships)
    //    IRelationship IRelationship<TNode>.Relate(TNode node, TNode neighbor)
    //    {
    //        //TODO:

    //        //node.Edges(this);
    //        throw new NotImplementedException();
    //    }

    //    //public bool Relate(TNode node, TNode neighbor)
    //    //{
    //    //    subject = node;
    //    //    var predicate = ToExpression().Compile();
    //    //    return predicate(node, neighbor);
    //    //}

    //    public Relationship<TNode> And(Relationship<TNode> relationship) => new AndRelationship<TNode>(this, relationship);

    //    public Relationship<TNode> Or(Relationship<TNode> relationship) => new OrRelationship<TNode>(this, relationship);
    //}

    //public abstract class Relationship<TNode, Other> : IRelationship<TNode, Other>
    //    where Other : INode
    //    where TNode : INode
    //{
    //    //private INode subject;

    //    public abstract Expression<Func<TNode, Other, bool>> ToExpression();

    //    public bool AreRelated(TNode node, Other other)
    //    {
    //        var predicate = ToExpression().Compile();
    //        return predicate(node, other);
    //    }

    //    IRelationship IRelationship<TNode, Other>.Relate(TNode node, Other neighbor) => throw new NotImplementedException();

    //    //public bool Relate(TNode node, Other other)
    //    //{
    //    //    //subject = node;
    //    //    var predicate = ToExpression().Compile();
    //    //    return predicate(node, other);
    //    //}

    //    public Relationship<TNode, Other> And(Relationship<TNode, Other> relationship)
    //        => new AndRelationship<TNode, Other>(this, relationship);

    //    public Relationship<TNode, Other> Or(Relationship<TNode, Other> relationship)
    //        => new OrRelationship<TNode, Other>(this, relationship);
    //}

    //internal sealed class RelationshipAdapter
    //{
    //    private IRelationship relationship;

    //    //private INode subject;

    //    public RelationshipAdapter(IRelationship relationship) => this.relationship = relationship;

    //    public bool IsEdgeOf(INode subject)
    //    {
    //        //var relationships = subject.Edges.SelectMany(edge => edge.Relationships).ToArray();
    //        //return relationships.Any(relationship => relationship.IsEdgeOf(subject));
    //        //return this.IsEdgeOf(subject);
    //        //throw new NotImplementedException();
    //    }
    //}
}