using System;
using System.Linq;

namespace Movies.Data
{
    internal interface ISession : IDisposable
    {
        /// <summary>
        /// Dummy Neo4j Queryable
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query<T>();
    }
}