/* Author: Michael Preston
 * License: N/A
 * Last Update: 8/30/2018
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared
{
    /// <summary>
    /// The following is a DSL for extracting class object from regular expressions and strings.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Extractor<T>
    {
        readonly BindingFlags defaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;
        Regex regex;
        PropertyInfo[] properties;
        StringBuilder errors = new StringBuilder(0);
        Dictionary<string, Regex> patternsMap;
        T result;
        T none = default(T);

        public string Errors => errors?.ToString();
        public T Value => HasValue ? result : throw new InvalidOperationException($"{typeof(T).Name} does not have a value!");

        public bool HasValue => result != null;
        public RegexOptions? ExtractionOptions { get; set; }

        private Extractor() => patternsMap = InitializeMap();

        public static Extractor<T> Create() => new Extractor<T>();

        public Extractor<T> WithOptions(RegexOptions options)
        {
            ExtractionOptions = options;
            return this;
        }

        public Extractor<T> SetPropertyPattern(string pattern) => this;

        public Extractor<T> Extract(string text) => Extract(text.Split('\n', '\r'));

        public U Case<U>(Func<T, U> some, Func<U> none) => HasValue
            ? some(Value)
            : none();

        public Extractor<T> Case(Action<T> some, Action none)
        {
            if (HasValue)
            {
                some(Value);
            }
            else
            {
                none();
            }

            return this;
        }

        public Extractor<T> IfSome(Action<T> some)
        {
            if (HasValue)
            {
                some(Value);
            }

            return this;
        }

        public T ValueOrDefault(T @default) => HasValue ? Value : @default;

        public T ValueOrDefault() => HasValue ? Value : none;

        public T ValueOrThrow(Exception exception) => HasValue ? Value : throw exception;

        /// <summary>
        /// Source: https://mikhail.io/2018/07/monads-explained-in-csharp-again/
        /// Sample Usage:
        /// Maybe<Shipper> shipperOfLastOrderOnCurrentAddress =
        ///     repo.GetCustomer(customerId)
        ///     .Bind(c => c.Address)
        ///     .Bind(a => repo.GetAddress(a.Id))
        ///     .Bind(a => a.LastOrder)
        ///     .Bind(lo => repo.GetOrder(lo.Id))
        ///     .Bind(o => o.Shipper);
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public Extractor<U> Bind<U>(Func<T, Extractor<U>> func) where U : class => HasValue
            ? func(result)
            : Extractor<U>.Create(); //same as None

        /// <summary>
        /// Main Extractor method
        /// Can use as a monad
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private Extractor<T> Extract(string[] lines)
        {
            properties = GetProperties();
            ExtractionOptions = ExtractionOptions.HasValue ? ExtractionOptions : RegexOptions.Singleline;
            var instance = (T)Activator.CreateInstance(typeof(T));
            Match match;
            PropertyInfo property;
            string line;

            for (int propIndex = 0; propIndex < properties.Length; propIndex++)
            {
                property = properties[propIndex];
                regex = patternsMap.GetValue(property.Name);

                for (int index = 0; index < lines.Length; index++)
                {
                    line = lines[index];

                    try
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        match = regex?.Match(line);

                        if (match == null || !match.Success)
                            continue;

                        string value = match?.Groups[property.Name]?.Value?.Trim();

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            property.SetValue(instance, TypeDescriptor.GetConverter(property.PropertyType)
                                .ConvertFrom(value), null);
                        }
                        else
                        {
                            property.SetValue(instance, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        //non-fatal exception.
                        errors.AppendLine(ex.Message);
                        continue;
                    }
                }
            }

            result = instance;
            return this;
        }

        private PropertyInfo[] GetProperties() => typeof(T)
                .GetProperties(defaultBindingFlags);

        private Dictionary<string, Regex> InitializeMap() => (properties ?? GetProperties()).Aggregate(new Dictionary<string, Regex>(), (dictionary, property) =>
                                                                       {
                                                                           var attributes = property.GetCustomAttributes(true);
                                                                           if (attributes.Length != 0)
                                                                           {
                                                                               var value = attributes
                                                                                    .Select(a => a as PatternAttribute)
                                                                                    .Single()
                                                                                    .Value;

                                                                               dictionary.Add(property.Name, new Regex(value));
                                                                           }
                                                                           return dictionary;
                                                                       });

        /// Set value on instance
        public Extractor<T> SetValue<TProperty>(Expression<Func<TProperty>> propertyExpression, TProperty value)
        {
            var lambdaExpression = propertyExpression as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Invalid lambda expression", "Lambda expression return value can't be null");
            }

            string propertyName = GetPropertyNameFrom(lambdaExpression);

            if (!HasValue)
            {
                throw new Exception($"Cannot set property {propertyName} value because this instance of <{typeof(T).Name}> is null.");
            }

            return SetValue(propertyName, value);
        }

        public Extractor<T> SetValue<TProperty>(string propertyName, TProperty value)
        {
            var propertyToChange = properties?.Single(p => p.Name.Equals(propertyName));

            if (propertyToChange == null)
            {
                throw new NullReferenceException("Could not find property {}!");
            }

            var storedValue = propertyToChange.GetValue(result);

            if (!Equals(storedValue, value))
            {
                propertyToChange.SetValue(result, value);
            }

            return this;
        }

        protected string GetPropertyNameFrom(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Expression must be a MemberExpression.", propertyExpression.Name);
            }

            return memberExpression.Member.Name;
        }

        protected string GetPropertyNameFrom(LambdaExpression lambdaExpression)
        {
            MemberExpression memberExpression;

            if (lambdaExpression.Body is UnaryExpression)
            {
                var unaryExpression = lambdaExpression.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambdaExpression.Body as MemberExpression;
            }

            return memberExpression.Member.Name;
        }
    }
}
