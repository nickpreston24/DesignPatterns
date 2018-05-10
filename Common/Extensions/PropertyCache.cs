using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Common
{
    public class PropertyCache
    {
        public PropertyInfo[] this[Type type] => Cache.TryGetValue(type, out PropertyInfo[] properties)
            ? properties
            : type.Cache()[type];

        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache { get; private set; } = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public int Count => Cache.Count;

        internal static bool TryAdd(Type type, PropertyInfo[] propertyInfo) => Cache.TryAdd(type, propertyInfo);
    }
}
