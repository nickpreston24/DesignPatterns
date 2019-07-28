using DesignPatterns;
using System;
using System.Collections.Generic;

namespace Graphs
{
    //Iterable collection
    public interface IGraph : IIterable<INode>, IDisposable
    {
        ICollection<INode> Nodes { get; }
    }
}