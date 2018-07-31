using System.Collections.Concurrent;

namespace System.Reflection
{
    public class PropertyCache
    {
        public PropertyInfo[] this[Type type] => Cache.TryGetValue(type, out var properties)
            ? properties
            : type.Cache()[type];

        public ConcurrentDictionary<Type, PropertyInfo[]> Cache { get; private set; } = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public int Count => Cache.Count;

        internal bool TryAdd(Type type, PropertyInfo[] propertyInfo) => Cache.TryAdd(type, propertyInfo);
    }
}
