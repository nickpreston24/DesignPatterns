using System.Collections.Generic;

namespace Graphs
{
    /// <summary>
    /// An Edge has a set of Relationships {WorksWith, Co-Starred, Married, etc.}
    /// </summary>
    public class Edge : IEdge
    {
        private bool disposedValue = false; // To detect redundant calls

        //private List<IRelationship<INode>> relationships = new List<IRelationship<INode>>(0);

        //public ICollection<IRelationship<INode>> Relationships
        //{
        //    get => relationships;
        //    set => relationships.AddRange(value);
        //}

        //public IRelationship<INode> this[int index]
        //{
        //    get => relationships[index];
        //    set => relationships[index] = value;
        //}

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                //Console.WriteLine($"Disposing { GetType().Name }...");
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    //foreach (var relationship in Relationships)
                    //{
                    //    relationship.Dispose();
                    //}
                }

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);

        //public override string ToString() => $"[Edge]{Relationships.Count}";
    }
}