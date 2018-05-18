using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Shared
{
    public static partial class CommonExtensions
    {
        public static PropertyCache propertyCache = new PropertyCache();

        public static ConcurrentDictionary<Type, PropertyInfo[]> Cache(this Type type)
        {
            //propertyCache.TryAdd(type, type.GetProperties());
            //return propertyCache;
            throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);

        }
    }
}
