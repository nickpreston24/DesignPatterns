using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace DesignPatterns
{
    public static partial class Extensions
    {
        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache(this Type type)
        {
            PropertyCache.TryAdd(type, type.GetProperties());
            return PropertyCache.Cache;
        }
    }
}
