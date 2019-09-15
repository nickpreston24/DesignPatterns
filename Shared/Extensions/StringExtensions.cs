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
        public static string Append(this string origin, string text)
            => new StringBuilder(origin).Append(text).ToString();

        public static string Reverse(this string text) => new string(text.ToCharArray().Reverse().ToArray());

        public static string Clean(this string text) => Regex.Replace(text, @"[-$_!@#?]", "").Trim();

        public static string SplitCamelCase(this string text) =>
            Regex.Replace(
            Regex.Replace(
            Regex.Replace(text.Clean(),
                    @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                    @"(\p{Ll})(\P{Ll})", "$1 $2"),
                @"\s+", " ");

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
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, @object);
                return writer.ToString();
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
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (!converter.CanConvertFrom(typeof(string)))
                {
                    throw new ArgumentException("Type does not have a TypeConverter from string", "T");
                }
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return
                        Regex.Matches(text, regexPattern)
                        .Cast<Match>()
                        .Select(match => match.ToString())
                        .Select(txt => (T)converter.ConvertFrom(txt))
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
        /// <param name="length">Max number of characters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
            => value != null
            && value.Length > length
                ? value.Substring(value.Length - length)
                : value;

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of characters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
            => value != null
            && value.Length > length
                ? value.Substring(0, length)
                : value;
    }
}