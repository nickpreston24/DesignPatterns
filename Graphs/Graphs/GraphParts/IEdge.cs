using System;
using System.Collections.Generic;

namespace Graphs
{
    // TODO: Figure out if you want to make Edge a collection of 3 types of IRelationship, <>, <INode>, <INode, INode>
    public interface IEdge : IDisposable
    {
        //ICollection<IRelationship> Relationships { get; set; }
        //ICollection<IRelationship<INode>> Relationships { get; set; }

        //Todo: Other properties, similar to how neo4j adds props to Edges
    }
}