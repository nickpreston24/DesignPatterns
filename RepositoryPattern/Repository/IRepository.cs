using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DesignPatterns
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        TEntity Remove(TEntity entity);
        IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);
    }
    public abstract class Entity
    {
        public int Id { get; protected set; }
    }
}