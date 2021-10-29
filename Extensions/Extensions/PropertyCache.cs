using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Extensions
{
/// <summary>
    /// Backing cache for all extensions.
    /// </summary>
    public static partial class Extensions
    {
        public static PropertyCache propertyCache = new PropertyCache();

        public static PropertyInfo[] Stash(this PropertyCache cache, Type type) => cache[type];
        public static PropertyCache Stash<T>(this PropertyCache cache)
        {
            cache.Stash(typeof(T));
            return cache;
        }

        public static PropertyCache Flush(this PropertyCache cache)
        {
            cache?.propertyMap?.Clear();
            return cache;
        }

        //https://damieng.com/blog/2012/10/29/8-things-you-probably-didnt-know-about-csharp
        public static IEnumerable<object> GetValues<TKey>(this Dictionary<TKey, object> dictionary, params TKey[] keys) => keys.Select(key => dictionary[key]);
    }

    public class PropertyCache
    {
        private PropertyInfo[] None = Enumerable.Empty<PropertyInfo>().ToArray();

        internal Dictionary<Type, PropertyInfo[]> propertyMap = new Dictionary<Type, PropertyInfo[]>();
        public int Count => propertyMap.Count;

        internal PropertyInfo[] AddAsProp(Type type)
        {
            var props = type.GetProperties();
            propertyMap.Add(type, props);
            return props;
        }

        //public PropertyInfo[] this[Type key] => propertyMap.TryGetValue(key, out var properties)
        //    ? properties : key.GetProperties() ?? None;

        public PropertyInfo[] this[Type key]
        {
            get
            {
                var value = propertyMap.TryGetValue(key, out var properties);
                return value ? properties : AddAsProp(key);
            }
        }

        public static PropertyInfo[] Create<T>() => new PropertyCache().Stash(typeof(T));

        public static PropertyCache Create() => new PropertyCache();
    }
}
