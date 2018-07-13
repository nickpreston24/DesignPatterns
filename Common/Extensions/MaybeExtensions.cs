using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value) where T : class => value != null
                ? Maybe.Some(value)
                : Maybe<T>.None;

        public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct => nullable.HasValue
                ? Maybe.Some(nullable.Value)
                : Maybe<T>.None;

        public static Maybe<string> NoneIfEmpty(this string s) => string.IsNullOrEmpty(s)
                ? Maybe<string>.None
                : Maybe.Some(s);

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> self) where T : class => self.FirstOrDefault().ToMaybe();

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T?> self) where T : struct => self.FirstOrDefault().ToMaybe();

        /// <summary>
        /// Return function is implemented with a combination of a public constructor which accepts Some value (notice that null is not allowed) and a static None method returning an object of 'no value'. Return extension method combines both of them in one call.
        /// </summary>
        public static Maybe<T> Return<T>(this T value) where T : class
        {
            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }
    }
}


