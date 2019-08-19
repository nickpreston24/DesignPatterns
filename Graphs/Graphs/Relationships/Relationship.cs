using DesignPatterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    /// <summary>
    /// A Relationship should:
    /// Tell if it is an Relationship on a given Node.
    /// Chain to another specification
    /// Compile its expression(s)
    /// </summary>
    public abstract class Relationship<TNode> : Specification<TNode>, IEdge<TNode>
        where TNode : INode
    {
        private Func<TNode, bool> condition;
        private INode subject;

        protected Relationship(INode target)
        {
            Neighbor = target;
            condition = condition ?? Condition().Compile();
        }

        public string Name { get; set; }

        public INode Neighbor { get; set; }

        public INode Subject => subject;

        public bool HasSubject(TNode node) => Subject.Equals(node);

        public bool IsEdgeOf(INode node) =>
            //return node.Relationships.Any(edge => edge.Subject.Equals(subject));
            //return this.Subject.Relationships.Any(x => x.Neighbor.Equals(node));
            //node.Relationships.Any(relationship => relationship.Equals(this));
            Neighbor.Equals(node) || Subject.Equals(node);

        protected Relationship<TNode> Link(INode subject)
        {
            this.subject = subject;
            return this;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

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

    //    // Relate a given node to his neighbor by establishing a Relationship between the two
    //    // Add this Relationship to both nodes' Relationships (collection of Relationships)
    //    IRelationship IRelationship<TNode>.Relate(TNode node, TNode neighbor)
    //    {
    //        //TODO:

    //        //node.Relationships(this);
    //        throw new NotImplementedException();
    //    }

    //    //public bool Relate(TNode node, TNode neighbor)
    //    //{
    //    //    subject = node;
    //    //    var predicate = ToExpression().Compile();
    //    //    return predicate(node, neighbor);
    //    //}

    //    public Relationship<TNode> And(Relationship<TNode> Relationship) => new AndRelationship<TNode>(this, Relationship);

    //    public Relationship<TNode> Or(Relationship<TNode> Relationship) => new OrRelationship<TNode>(this, Relationship);
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

    //    public Relationship<TNode, Other> And(Relationship<TNode, Other> Relationship)
    //        => new AndRelationship<TNode, Other>(this, Relationship);

    //    public Relationship<TNode, Other> Or(Relationship<TNode, Other> Relationship)
    //        => new OrRelationship<TNode, Other>(this, Relationship);
    //}

    //internal sealed class RelationshipAdapter
    //{
    //    private IRelationship Relationship;

    //    //private INode subject;

    //    public RelationshipAdapter(IRelationship Relationship) => this.Relationship = Relationship;

    //    public bool IsRelationshipOf(INode subject)
    //    {
    //        //var Relationships = subject.Relationships.SelectMany(Relationship => Relationship.Relationships).ToArray();
    //        //return Relationships.Any(Relationship => Relationship.IsRelationshipOf(subject));
    //        //return this.IsRelationshipOf(subject);
    //        //throw new NotImplementedException();
    //    }
    //}
}