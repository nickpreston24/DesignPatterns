using System;
using System.Collections.Concurrent;

namespace DesignPatterns.Singleton
{
    public class SingletonSelector
    {
        public static ConcurrentDictionary<Type, object> _singletons = new ConcurrentDictionary<Type, object>();
        //todo: make this into a multiton with string/type keys for selecting singletons.
    }
}
