using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Extensions
{
    public class PropertyCache //todo: make this a singleton, and concurrent dictionary
    {
        public static Dictionary<Type, PropertyInfo[]> _propertyCache = new Dictionary<Type, PropertyInfo[]>();
        public PropertyInfo[] this[Type index] => _propertyCache.TryGetValue(index, out PropertyInfo[] properties) ? properties : index.Cache()[index];
    }
}
