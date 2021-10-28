using IncidentInspector.Core;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
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
}
