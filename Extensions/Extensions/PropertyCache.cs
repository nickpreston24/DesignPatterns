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


        private static readonly ConcurrentDictionary<Type, PropertyHelper[]> Cache
 = new ConcurrentDictionary<Type, PropertyHelper[]>();
 private static readonly MethodInfo CallInnerDelegateMethod =
 typeof(PropertyHelper).GetMethod(nameof(CallInnerDelegate), BindingFlags.NonPublic | BindingFlags.Static);
 public string Name { get; set; }
 public Func<object, object> Getter { get; set; }
 public static PropertyHelper[] GetProperties(Type type)
 => Cache
 .GetOrAdd(type, _ => type
 .GetProperties()
 .Select(property =>
 {
 var getMethod = property.GetMethod;
 var declaringClass = property.DeclaringType;
 var typeOfResult = property.PropertyType;
 // Func<Type, TResult>
 var getMethodDelegateType = typeof(Func<,>).MakeGenericType(declaringClass, typeOfResult);
 // c => c.Data
 var getMethodDelegate = getMethod.CreateDelegate(getMethodDelegateType);
 // CallInnerDelegate<Type, TResult>
 var callInnerGenericMethodWithTypes = CallInnerDelegateMethod
 .MakeGenericMethod(declaringClass, typeOfResult);
 // Func<object, object>
 var result = (Func<object, object>)callInnerGenericMethodWithTypes.Invoke(null, new[] {getMethodDelegate});
 return new PropertyHelper
 {
 Name = property.Name,
 Getter = result
 };
 })
 .ToArray());
 // Called via reflection.
 private static Func<object, object> CallInnerDelegate<TClass, TResult>(
 Func<TClass, TResult> deleg)
 => instance => deleg((TClass)instance);

    }

    /// <summary>
	/// Backing property cache for all extensions.
	/// </summary>
	public static partial class PropertyExtensions
	{
		public static PropStash propertyCache = new PropStash();

		//public static PropertyInfo[] Stash(this PropStash cache, Type type) => cache[type];
		public static PropStash Stash<T>(this PropStash cache)
		{
			cache.Stash<T>();
			return cache;
		}

		public static PropStash Flush(this PropStash cache)
		{
			cache?.propertyMap?.Clear();
			return cache;
		}

		//https://damieng.com/blog/2012/10/29/8-things-you-probably-didnt-know-about-csharp
		public static IEnumerable<object> GetValues<TKey>(this Dictionary<TKey, object> dictionary, params TKey[] keys) => keys.Select(key => dictionary[key]);
	}

    class PropertyHelper
	{

	}
}
