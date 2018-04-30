using DesignPatterns;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class Extensions
    {
        public static Maybe<T> ToMaybe<T>(this T value) where T : class => value != null
                ? Maybe.Some(value)
                : Maybe<T>.None;

        public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct => nullable.HasValue
                ? Maybe.Some(nullable.Value)
                : Maybe<T>.None;

        public static Maybe<string> NoneIfEmpty(this string text) => string.IsNullOrEmpty(text)
                ? Maybe<string>.None
                : Maybe.Some(text);

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> self) where T : class => self.FirstOrDefault().ToMaybe();

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T?> self) where T : struct => self.FirstOrDefault().ToMaybe();
    }
}

