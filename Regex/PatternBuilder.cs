using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexBuilder
{
    /// <summary>
    /// Create reusable Regex patterns
    /// Patterns may be validated and generated from known subpatterns
    /// </summary>
    public class PatternBuilder<T> where T : IRegexable
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

        private StringBuilder builder = new StringBuilder();
        private Regex pattern;
        private PropertyInfo[] properties = typeof(T).GetProperties();
        //private Type type = typeof(T);

        public PatternBuilder(StringBuilder builder) => this.builder = builder ?? new StringBuilder();

        public PatternBuilder(Regex pattern) => this.pattern = pattern;

        public PatternBuilder(string pattern)
            : this(new StringBuilder(pattern))
        {
        }

        public PatternBuilder()
        {
        }

        public static PatternBuilder<T> Create() => new PatternBuilder<T>();

        public Regex Pattern { get => pattern ?? ToRegex(); }

        //Naively build from class T:
        public PatternBuilder<T> Add<U>()
            where U : new()
        {
            string pattern = Extensions.GenerateRegex<U>();
            builder.Append(pattern);
            return this;
        }

        private string GenerateRegex()
        {
            foreach (var property in properties)
                builder.Append($@"(?<{property.Name}>{naiveRegexPatterns[property.PropertyType]})\s*");

            builder.Length -= 3;
            return builder.ToString();
        }

        private Regex ToRegex() => pattern = new Regex(GenerateRegex());
    }
}