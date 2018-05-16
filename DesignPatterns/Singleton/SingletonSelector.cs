using System;
using System.Collections.Concurrent;

namespace DesignPatterns
{
    public class SingletonSelector
    {
        private static readonly ConcurrentDictionary<Type, ISingleton> instances = new ConcurrentDictionary<Type, ISingleton>();

        private SingletonSelector() { }

        public static T GetInstance<T>() where T : class, ISingleton
        {
            lock (instances)
            {
                var key = typeof(T);

                if (!instances.TryGetValue(key, out var instance))
                {
                    instance = Singleton<T>.Instance as ISingleton;
                    instances.TryAdd(key, instance);
                }

                return instance as T;
            }
        }
    }

    ///Wrapper-Instance (Holds Implemenation)
    public class Selector<T>  //: SelectorImplementationBase
    {
        ISelector Implementation { get; set; }

        public ISingleton GetInstance<T>() where T : class, ISingleton
        {
            return Implementation.GetInstance<T>();
        }

        public Selector()
        {
            Implementation = new SelectorImplementation();
        }
    }

    public interface ISelector
    {
        ISingleton GetInstance<T>() where T : class, ISingleton;
    }

    ///Implements
    public class SelectorImplementation : ISelector
    {
        public virtual ISingleton GetInstance<T>() where T : class, ISingleton
        {
            return SingletonSelector.GetInstance<T>();
        }
    }
}
