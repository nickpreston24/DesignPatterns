using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace System
{
    public static partial class Extensions
    {
        public static string Append(this string input, string text) => new StringBuilder(input).Append(text).ToString();
        public static string Reverse(this string str) => new string(str.ToCharArray().Reverse().ToArray());

        public static string SplitCamelCase(this string str) => Regex.Replace(Regex.Replace(str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                    @"(\p{Ll})(\P{Ll})", "$1 $2");

        public static T DeserializeFromXML<T>(this string xml)
            where T : class
        {
            using (TextReader reader = new StringReader(xml))
            {
                return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
            }
        }

        public static string SerializeToXml<T>(this T @object)
            where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, @object);
                    return writer.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T DeserializeFromJson<T>(this string json) => JsonConvert.DeserializeObject<T>(json);

        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        // Extracts Object T from RegEx pattern
        // TODO: rewrite using the 'Slurp' method
        public static T Extract<T>(this string text, string regexPattern,
            bool matchExactPattern = true, bool matchCount = true)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return default(T);
            }

            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties?.Count() == 0)
            {
                return default(T);
            }

            var errors = new StringBuilder();

            try
            {
                var regex = new Regex(regexPattern, RegexOptions.Singleline);
                var match = regex.Match(text);

                if (!match.Success)
                {
                    errors.AppendLine($"No matches found! Could not extract a '{typeof(T).Name}' instance from regex pattern:\n{regexPattern}.\n");
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

                if (matchExactPattern && match.Groups.Count - 1 != properties.Length)
                {
                    if (matchCount)
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

                foreach (var property in properties)
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

                if (errors.Length >= 0)
                {
                    throw new Exception(errors.ToString());
                }

                return (T)instance;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// ExtractPrimitives
        ///
        /// Extracts all fields from a string that match a certain regex. 
        /// Will convert to desired type through a standard TypeConverter.
        /// Supports basic primative types ONLY
        /// Tip: Extract the 'T' type you expect (like int) to retrieve;
        /// (default to string if unsure)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="regexPattern"></param>
        /// <returns></returns>
        public static T[] ExtractPrimitives<T>(this string text, string regexPattern)
        {
            try
            {
                var tc = TypeDescriptor.GetConverter(typeof(T));
                if (!tc.CanConvertFrom(typeof(string)))
                {
                    throw new ArgumentException("Type does not have a TypeConverter from string", "T");
                }
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return
                        Regex.Matches(text, regexPattern)
                        .Cast<Match>()
                        .Select(f => f.ToString())
                        .Select(f => (T)tc.ConvertFrom(f))
                        .ToArray();
                }
                else
                {
                    return new T[0];
                }
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.ToString());
                Debug.WriteLine(errMsg);
                return new T[0];
            }
        }

        public static T ToEnum<T>(this string value) => !typeof(T).IsEnum ?
                throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}> Could not convert string '{value}' to type {typeof(T).Name}")
                : (T)Enum.Parse(typeof(T), value);

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static MatchCollection ExtractNestedFunctionsAndParams(this string txt)
        {
            string nestedFunctionsPattern = @"(?:[^,()]+((?:\((?>[^()]+|\((?<open>)|\)(?<-open>))*\)))*)+";
            var match = Regex.Match(txt, nestedFunctionsPattern);
            string innerArgs = match.Groups[1].Value;
            var matches = Regex.Matches(innerArgs, nestedFunctionsPattern);
            return matches;
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length) => value != null && value.Length > length ? value.Substring(value.Length - length) : value;

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length) => value != null && value.Length > length ? value.Substring(0, length) : value;
    }
}
