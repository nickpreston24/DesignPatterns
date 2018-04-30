using DesignPatterns;
using System.Collections.Concurrent;

namespace System.Reflection
{
    public static class Extensions
    {
        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache(this Type type)
        {
            PropertyCache.TryAdd(type, type.GetProperties());
            return PropertyCache.Cache;
        }
    }
}
