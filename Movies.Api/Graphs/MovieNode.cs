using System;
using System.Collections;
using System.Collections.Generic;

namespace Movies.Api.Graphs
{
    //composite element
    public class MovieNode : INode
    {
        public int Id { get; set; }
        public IList<IRelationship> Relationships { get; set; }
    }

    public interface INode
    {
        int Id { get; set; }
        IList<IRelationship> Relationships { get; set; }
    }

    //Dummy class for working relationships
    public class WorkedWith : IRelationship
    {

    }

    //Dummy interface for a Neo4j-style relationship
    public interface IRelationship
    {
    }

    //derived collection
    public class MovieGraph
    {
        internal object GetNodes() => throw new NotImplementedException();
    }

    //concrete collection
    public class Graph : IGraph
    {
        public object CreateIterator() => throw new NotImplementedException();
    }

    //iterable collection
    public interface IGraph
    {
        object CreateIterator();
    }

    /// <summary>
    /// From Refactoring Guru:
    /// </summary>
    //public abstract class Iterator : IIterable
    //{
    //    public abstract object Current { get; }

    //    public bool MoveNext()
    //    {
    //        //TODO: advance to next element (n+1)
    //        throw new NotImplementedException();
    //    }

    //    public void Reset()
    //    {
    //        //Todo: rewind to first element
    //        throw new NotImplementedException();
    //    }

    //    // Todo: return current element
    //    //public abstract object Current();

    //    // Todo: return current key of the element (like kvps)
    //    public abstract int Key();
    //    public IEnumerator GetEnumerator() => throw new NotImplementedException();
    //}

    //public abstract class IteratorAggregate : IEnumerable
    //{
    //    public abstract IEnumerator GetEnumerator();
    //}

    //Dummy Concrete iterator
    public class BFSIterator : IIterator<Movie>
    {
        private MovieGraph graph;

        public BFSIterator(MovieGraph movieGraph)
        {
            graph = movieGraph;
        }

        //public override object Current => graph.GetNodes();

        //public override int Key()
        //{
        //    throw new NotImplementedException();
        //}
    }

}
