using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace DesignPatterns
{
    public class PropertyCache
    {
        public PropertyInfo[] this[Type type] => Cache.TryGetValue(type, out PropertyInfo[] properties) ? properties : type.Cache()[type];
        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache { get; private set; } = new ConcurrentDictionary<Type, PropertyInfo[]>();
        public int Count => Cache.Count;

        internal static bool TryAdd(Type type, PropertyInfo[] propertyInfo) => Cache.TryAdd(type, propertyInfo);
    }

    public static partial class DynamicExtensions
    {
        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache(this Type type)
        {
            PropertyCache.TryAdd(type, type.GetProperties());
            return PropertyCache.Cache;
        }
    }

    //public class PropertyCache
    //{
    //    private static ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
    //    public PropertyInfo[] this[Type type] => _propertyCache.TryGetValue(type, out PropertyInfo[] properties) ? properties : type.Cache()[type];
    //    public int Count => _propertyCache.Count;
    //}
}
