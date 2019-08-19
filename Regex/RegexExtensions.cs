using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexBuilder
{
    public static partial class Extensions
    {
        private static Dictionary<Type, string> naiveRegexPatterns = new Dictionary<Type, string>()
        {
            [typeof(string)] = "[a-zA-Z]+",
            [typeof(char*)] = "[a-zA-Z]+",
            [typeof(int)] = @"\d+",
            [typeof(short)] = @"\d+",
            [typeof(uint)] = @"[^-]\d+",
            [typeof(double)] = @"\d+.\d{1,2}",
        };

        /// <summary>
        /// Extracts a class object from the specified regex pattern.
        /// </summary>
        public static T Extract<T>(this string text, string regexPattern,
            bool matchStrict = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            var properties = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties?.Length == 0)
                throw new ArgumentNullException($"No properties found for type {typeof(T).Name}");

            var errors = new StringBuilder();
            var regex = new Regex(regexPattern, RegexOptions.Singleline);
            var match = regex.Match(text);

            if (!match.Success)
            {
                errors.AppendLine($"No matches found! Could not extract a '{typeof(T).Name}' instance from regex pattern:\n{regexPattern}.\n");
                errors.AppendLine(text);

                var missing = properties.Select(property => property.Name)
                        .Except(regex.GetGroupNames());
                if (missing.Any())
                {
                    errors.AppendLine("Properties without a mapped Group:");
                    missing.Aggregate(errors, (result, name) => result.AppendLine(name));
                }

                if (errors.Length > 0)
                {
                    throw new Exception(errors.ToString());
                }
            }

            if (matchStrict && match.Groups.Count - 1 != properties.Length)
            {
                errors.AppendLine($"{MethodBase.GetCurrentMethod().Name}() " +
                    $"WARNING: Number of Matched Groups ({match.Groups.Count}) " +
                    $"does not equal the number of properties for the given class '{typeof(T).Name}'({properties.Length})!  " +
                    $"Check the class type and regex pattern for errors and try again.");

                errors.AppendLine("Values Parsed Successfully:");

                for (int groupIndex = 1; groupIndex < match.Groups.Count; groupIndex++)
                {
                    errors.Append($"{match.Groups[groupIndex].Value}\t");
                }
                errors.AppendLine();
                throw new Exception(errors.ToString());
            }

            object instance = Activator.CreateInstance(typeof(T));
            //object instance = Activator
            //    .CreateInstance(GetAssignableTypes<T>()
            //    .FirstOrDefault());

            foreach (var property in properties)
            {
                string value = match?
                    .Groups[property.Name]?
                    .Value?
                    .Trim();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    property.SetValue(instance, TypeDescriptor
                        .GetConverter(property.PropertyType)
                        .ConvertFrom(value), null);
                }
                else
                {
                    property.SetValue(instance, null, null);
                }
            }

            return (T)instance;
        }

        public static string GenerateRegex<T>()
        {
            return default(T).GenerateRegex<T>();
        }

        public static string GenerateRegex<T>(this T obj)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var builder = new StringBuilder(properties.Length);
            if (properties.Length == 0)
                throw new InvalidOperationException($"Reference type {type.Name} has no properties.");

            foreach (var property in properties)
                builder.Append($@"(?<{property.Name}>{naiveRegexPatterns[property.PropertyType]})\s*");

            builder.Length -= 3;
            return builder.ToString();
        }

        /// <summary>
        /// Gets the specified array value within the bounds, every time.
        /// </summary>
        public static string Get(this string[] array, int index)
            => array?.Length >= index + 1
                ? array?[index] ?? string.Empty
                : string.Empty;
    }
}