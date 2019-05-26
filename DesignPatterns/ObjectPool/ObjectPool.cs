using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns
{
    public class ObjectPool<T> : IDisposable
    {
        private ConcurrentBag<T> items = new ConcurrentBag<T>(Enumerable.Empty<T>());
        private Func<T> generator = () => default;

        public int Count => items != null ? items.Count : 0;
        public ObjectPool() => items = new ConcurrentBag<T>(Enumerable.Empty<T>());

        public ObjectPool(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                items.Add(item);
        }

        /// <summary>
        /// ObjectPool
        /// </summary>
        /// <param name="capacity">
        /// Intent: preload internal list with new T objects for immediate use w/o
        /// requesting heap allocation
        /// </param>
        public ObjectPool(int capacity = 1)
        {
            if (capacity < 0)
                capacity = Math.Abs(capacity);

            CreateObjects(capacity);
        }

        public ObjectPool(Func<T> objectGenerator) => generator = objectGenerator ?? throw new ArgumentNullException("objectGenerator");

        //
        /// DSL
        /// 
        ////
        public ObjectPool<T> SetGenerator(Func<T> objectGenerator)
        {
            generator = objectGenerator ?? throw new ArgumentNullException("objectGenerator");
            return this;
        }

        public ObjectPool<T> Add(T item)
        {
            items.Add(item);
            return this;
        }

        public ObjectPool<T> Add(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                items.Add(item);

            return this;
        }

        public T Get() => items.TryTake(out var obj) ? obj : generator();

        public IEnumerable<T> Get(int count = 1)
        {
            return count == 1 ? Enumerable.Repeat(Get(), 1) : Enumerable.Range(1, count)
                .Aggregate(new List<T>(), (result, item) =>
             {
                 result.Add(items.TryTake(out var obj) ? obj : default);
                 return result;
             });
        }

        public IEnumerable<T> GetRandom(int count = 1) => items.TakeRandom(count);

        private void CreateObjects(int capacity)
        {
            var collection = Enumerable.Range(1, capacity)
                            .Aggregate(new List<T>(), func: (result, item) =>
                            {
                                result.Add(Activator.CreateInstance<T>());
                                return result;
                            });

            items = new ConcurrentBag<T>(collection);
        }

        //Idea: https://stackoverflow.com/questions/3029818/how-to-remove-a-single-specific-object-from-a-concurrentbag
        //public bool Release(T specificItem)
        //{
        //    //TODO: implement a find() method that gets the matching item and pops it from the list, returning it.
        //    return false;
        //}

        #region IDisposable Implementation
        private bool _disposed;
        ~ObjectPool()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                //free managed objects that implement IDisposable only
                while (!items.IsEmpty)
                    items.TryTake(out var someItem);
            }

            //release any unmanaged objects here and set object references to null
            items = null;
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Implementation
    }
}
