namespace DesignPatterns
{
    /// <summary>
    /// SQL Server UnitOfWork
    /// todo: create abstract base for MongoDbUnit of work, or use strategy.
    /// </summary>
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected IDbConnection connection;

        protected UnitOfWork(IDbConnection connection) => this.connection = connection;

        public abstract void Complete();

        public abstract void Dispose();
    }
}