using System.Data.SqlClient;

namespace DesignPatterns
{
    /// <summary>
    /// SQL Server UnitOfWork
    /// todo: create abstract base for MongoDbUnit of work, or use strategy.
    /// </summary>
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected SqlConnection connection;

        protected UnitOfWork(string connectionString) => connection = new SqlConnection(connectionString);

        public abstract void Complete();

        public abstract void Dispose();
    }
}