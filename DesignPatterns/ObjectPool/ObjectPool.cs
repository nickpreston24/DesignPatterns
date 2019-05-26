using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DesignPatterns
{
    public class ObjectPool<T>
    {
        public List<T> Instances { get; set; } = new List<T>();
        public int Count => Instances.Count;


        public ObjectPool() { }
        public ObjectPool(IEnumerable<T> items) => Instances = items.ToList();
        public ObjectPool(int preload)
        {
            Instances = Enumerable.Range(1, preload)
                     .Aggregate(seed: new List<T>(preload), func: (result, item) =>
                     {
                         result.Add(Activator.CreateInstance<T>());
                         return result;
                     });
        }

        public void Add(T item) => Instances.Add(item);
        public void Add(IEnumerable<T> items) => Instances.AddRange(items);

        //TODO: implement a find() method that gets the matching item and pops it from the list, returning it.
        public T Release(T item)
        {
            Instances.Remove(item);
            return item;
        }

        // TODO: implement the same for an IEnumerable set as well.
    }
}
