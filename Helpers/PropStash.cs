using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IncidentInspector.Core
{
	/// <summary>
	/// Store Properties of one or more types
	/// </summary>
	public class PropStash
	{
		private PropertyInfo[] None = Enumerable.Empty<PropertyInfo>().ToArray();

		internal Dictionary<Type, PropertyInfo[]> propertyMap = new Dictionary<Type, PropertyInfo[]>();
		public int Count => propertyMap.Count;

		/// <summary>
		/// Stashes the properties of a given type.
		/// Properties are accessible
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		internal PropertyInfo[] Stash<T>(T item)
		{
			Type type = typeof(T);
			PropertyInfo[] props = type.GetProperties();
			propertyMap.Add(type, props);
			return props;
		}

		public PropStash Stash<T>()
		{
			Type type = typeof(T);
			PropertyInfo[] props = type.GetProperties();
			propertyMap.Add(type, props);
			return this;
		}

		//public PropertyInfo[] this[Type key] => propertyMap.TryGetValue(key, out var properties)
		//    ? properties : key.GetProperties() ?? None;

		public PropertyInfo[] this[Type key]
		{
			get
			{
				bool value = propertyMap.TryGetValue(key, out PropertyInfo[] properties);
				return value ? properties : Stash(key);
			}
		}

		public static PropStash Create() => new PropStash();
	}
}
