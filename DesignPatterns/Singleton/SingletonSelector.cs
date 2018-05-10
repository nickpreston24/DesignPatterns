using System;
using System.Collections.Concurrent;

namespace DesignPatterns
{
    public class SingletonSelector
    {
        private static readonly ConcurrentDictionary<Type, ISingleton> _instances = new ConcurrentDictionary<Type, ISingleton>();

        private SingletonSelector() { }

        public static T GetInstance<T>() where T : class, ISingleton
        {
            lock (_instances)
            {
                var key = typeof(T);

                if (!_instances.TryGetValue(key, out var instance))
                {
                    instance = Singleton<T>.Instance as ISingleton;
                    _instances.TryAdd(key, instance);
                }

                return instance as T;
            }
        }
    }
}
