using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Extensions
{
	/// <summary>
	/// Extensions for the System.Regex pattern matching, replacing.
	/// Great for parsing ANYTHING.
	/// </summary>
	public static class RegexExtensions
	{
		/// <summary>
		/// Replaces all entries within the mappings for a given text.
		/// </summary>        
		public static string ReplaceAll(this string text, Dictionary<string, string> mapping)
		{
			// Non-throwing
			if (string.IsNullOrEmpty(text))
				return string.Empty;

			string pattern = string.Join("|", mapping.Keys
								.Select(keyName => keyName.ToString())
								.ToArray());
			return mapping is null || mapping.Count == 0
				? text
				: Regex.Replace(text, pattern,
			match =>
			{
				mapping.TryGetValue(match.Value, out string val);
				//return val ?? string.Empty;
				return string.IsNullOrWhiteSpace(val)
					? string.Empty
					: val;
			});
		}


		/// <summary>
		/// Extracts Class Object T from RegEx pattern.
		///    For example, a Person with FullName and Age might look something like:
		///    @"Name:\s*(?<Name>(\w+\s*)*).*Age:\s*(?<Age>(\d+))"
		///    
		///		With a line or row looking like:
		///		' Name: Kendra Williams			Age: 42 '
		///		
		///    This method will assign values from lines directly to a given class!  No more weird substring wizardry!
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="regex"></param>
		/// <param name="text"></param>
		/// <param name="matchExact"></param>
		/// <param name="warnOnUnevenCounts"></param>
		/// <returns></returns>
		public static T Extract<T>(this Regex regex
			, string text
			, bool matchExact = true
			, bool warnOnUnevenCounts = false
			)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return default(T);
			}

			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			if (properties?.Count() == 0)
			{
				return default(T);
			}

			StringBuilder errors = new StringBuilder();

			try
			{
				Match match = regex.Match(text);

				if (!match.Success)
				{
					errors.AppendLine($"No matches found! Could not extract a '{typeof(T).Name}' instance from regex pattern.");
					errors.AppendLine(text);
					errors.AppendLine("Properties without a mapped Group:");
					properties.Select(property => property.Name)
							.Except(regex.GetGroupNames())
							.ToList()
							.ForEach(name => errors.Append(name + '\t'));
					errors.AppendLine();

					if (errors.Length > 0)
					{
						throw new Exception(errors.ToString());
					}
				}

				if (matchExact && match.Groups.Count - 1 != properties.Length)
				{
					if (warnOnUnevenCounts)
					{
						errors.AppendLine($"{MethodBase.GetCurrentMethod().Name}() WARNING: Number of Matched Groups ({match.Groups.Count}) does not equal the number of properties for the given class '{typeof(T).Name}'({properties.Length})!  Check the class type and regex pattern for errors and try again.");

						errors.AppendLine("Values Parsed Successfully:");
					}

					for (int groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
					{
						errors.Append($"{match.Groups[groupIndex].Value}\t");
					}

					errors.AppendLine();

					if (errors.Length >= 0)
					{
						throw new Exception(errors.ToString());
					}
				}

				//object instance = Activator.CreateInstance(typeof(T));
				object instance = Activator.CreateInstance(GetAssignableTypes<T>().FirstOrDefault());
				//T instance = default(T);

				foreach (PropertyInfo property in properties)
				{
					try
					{
						string value = match?.Groups[property.Name]?.Value?.Trim();

						if (!string.IsNullOrWhiteSpace(value))
						{
							property.SetValue(instance, TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value), null);
						}
						else
						{
							property.SetValue(instance, null, null);
						}
					}
					catch (Exception ex)
					{
						errors.AppendLine(ex.ToString());
						continue;
					}
				}

				if (errors.Length > 0)
				{
					throw new Exception(errors.ToString());
				}

				return (T)instance;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public static IEnumerable<Type> GetAssignableTypes<T>()
		{
			try
			{
				Type[] assignableTypes = (from type in Assembly.Load(typeof(T).Namespace).GetExportedTypes()
									   where !type.IsInterface && !type.IsAbstract
									   where typeof(T).IsAssignableFrom(type)
									   select type).ToArray();

				return assignableTypes;
			}
			catch (Exception)
			{
				throw;
			}
		}

		//public static Dictionary<Type, PropertyInfo[]> Cache(this Type type)
		//{
		//	PropertyCache.propertyCache.Add(type, type.GetProperties());
		//	return PropertyCache.propertyCache;
		//}
	}

	public static partial class Serialization
	{
		public static T Deserialize<T>(this string xml)
		{
			using (StringReader reader = new StringReader(xml))
			{
				T result = !string.IsNullOrWhiteSpace(xml)
					? (T)new XmlSerializer(typeof(T)).Deserialize(reader)
					: default;
				return result;
			}
		}
	}


	#region WIP
	public class RegexBuilder<Model>
	{
		private Dictionary<Expression<Func<Type, string>>, string> cachedPatterns = null;

		public RegexOptions Options { get; private set; }

		/// <summary>
		/// 1) Start of pattern
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static RegexBuilder<Model> Start(Expression<Func<Model, string>> selector, RegexOptions options)
		{
			RegexBuilder<Model> builder = new RegexBuilder<Model>();
			builder.Options = options;
			// TODO: Compile expression down to <type.property, pattern>.
			return builder;
		}

		// 2) Append() new sub-pattern, making sure to never add a prop more than once.

		// 3) End of pattern (end chars only)

		// 4) Compile()

	}
	#endregion WIP

}
