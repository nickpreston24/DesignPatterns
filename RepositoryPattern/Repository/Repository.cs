using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DesignPatterns
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected IDbConnection connection;
        protected List<TEntity> collection;

        protected Repository(IDbConnection connection) => this.connection = connection;

        public abstract IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        public abstract TEntity Get(int id);

        public abstract IEnumerable<TEntity> GetAll();

        public abstract TEntity Add(TEntity entity);

        public abstract IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

        public abstract TEntity Remove(TEntity entity);

        public abstract IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);
                        
        #region Dispose
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)            
                return;
            
            if (disposing)
            {
                connection.Dispose();
            }

            isDisposed = true;
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Repository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion Dispose

    }    
}