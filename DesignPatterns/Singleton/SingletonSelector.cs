using System;
using System.Collections.Concurrent;

namespace DesignPatterns.Singleton
{
    public class SingletonSelector
    {
        public static ConcurrentDictionary<Type, object> _singletons = new ConcurrentDictionary<Type, object>();
        //todo: 
    }
}
