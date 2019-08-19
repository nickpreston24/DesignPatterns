using System.Collections.Concurrent;

namespace System.Reflection
{
    public static partial class Extensions
    {
        public static PropertyCache propertyCache = new PropertyCache();

        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache(this Type type)
        {
            propertyCache.TryAdd(type, type.GetProperties());
            return propertyCache.Cache;
        }
    }
}