using System;
using System.Collections.Generic;

namespace Graphs
{
    public interface INode : IDisposable
    {
        int Id { get; set; }
        ICollection<IEdge> Edges { get; }
    }
}