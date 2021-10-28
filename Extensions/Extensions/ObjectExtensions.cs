using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Extensions
{

	/// <summary>
	/// Extensions for any class / object's usage.
	/// </summary>
	public static partial class ObjectExtensions
	{

		//private static JsonConverter jsonConverter = new Newtonsoft.Json.Converters.StringEnumConverter();
		//private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		//{
		//	Converters = new List<JsonConverter> { jsonConverter },
		//	NullValueHandling = NullValueHandling.Include,
		//	ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		//};

		/// <summary>
		/// Prints any object as JSON
		/// </summary>
		/// <typeparam name="T">Type of the object can be anything</typeparam>
		/// <param name="obj"></param>
		/// <param name="displayName">An optional title to display over the json results</param>
		/// <param name="showNulls">Indicates whether to show null JSON values or not</param>
		/// <returns></returns>
		public static T Dump<T>(this T obj, string displayName = null, bool showNulls = true)
		{
			if (obj != null)
			{
				if (string.IsNullOrWhiteSpace(displayName))
				{
					displayName = obj.GetType().Name;
				}


				JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
				if (!showNulls)
				{
					// Old Newtonsoft version:
					//jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

					//New MS version:
					options.IgnoreNullValues = true;
				}

				// Old Newtonsoft version:
				//var prettyJson = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);

				string prettyJson = JsonSerializer.Serialize(obj, options);
				Debug.WriteLine($"{displayName}:\n{prettyJson}");
			}
			else if (obj == null && !string.IsNullOrWhiteSpace(displayName))
			{
				Debug.WriteLine(string.Format($"Object '{displayName}' is null.")); //Optional
				return default;
			}

			return obj;
		}

		/// <summary>
		/// Implement: https://www.codemag.com/Article/1905041/Immutability-in-C
		///				https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net
		///				
		/// Inspiration: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/with-expression
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static T With<T>(this T original, Action<T> mods) where T : class
		{
			throw new NotImplementedException();
		}

		//public static T Set<T>(this IEnumerable<T> self, Func<T, T> action)
		//  where T : class
		//{
		//	return self?.Select(item => item != null ? action(item) : null);
		//}

		/// <summary>
		/// Set an object value, but with side-effects allowed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static T Set<T>(this T self, Func<T, T> action)
		  where T : class
			  => self != null ? action(self) : default;

		/// <summary>
		/// Set an object's value by function
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static T Set<T>(this T self, Action<T> action)
		   where T : class
		{
			if (self == null)
				return default;

			action(self);
			return self;
		}

		public static IEnumerable<T> Set<T>(this IEnumerable<T> collection, Action<T> action)
		   where T : class => collection.Select(item => item.Set(action));


		public static IEnumerable<T> Set<T>(this IEnumerable<T> collection, Func<T, T> action)
		  where T : class => collection.Select(item => item.Set(action));
	}
}
