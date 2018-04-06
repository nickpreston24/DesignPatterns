using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns
{
    public struct Maybe<T>
    {
        readonly IEnumerable<T> values;

        public static Maybe<T> Some(T value) => value != null
                    ? new Maybe<T>(new[] { value })
                    : throw new InvalidOperationException();

        public static Maybe<T> None => new Maybe<T>(new T[0]);

        Maybe(IEnumerable<T> values) => this.values = values;

        public bool HasValue => values != null && values.Any();

        public T Value => HasValue
                    ? values.Single()
                    : throw new InvalidOperationException("Maybe does not have a value");

        public T ValueOrDefault(T @default) => HasValue
                    ? values.Single()
                    : @default;

        public T ValueOrThrow(Exception exception) => HasValue
                    ? Value
                    : throw exception;

        public U Case<U>(Func<T, U> some, Func<U> none)
        {
            return HasValue
                    ? some(Value)
                    : none();
        }

        public void Case(Action<T> some, Action none)
        {
            if (HasValue)
            {
                some(Value);
            }
            else
            {
                none();
            }
        }

        public void IfSome(Action<T> some)
        {
            if (HasValue)
            {
                some(Value);
            }
        }

        public Maybe<U> Map<U>(Func<T, Maybe<U>> map) => HasValue
                    ? map(Value)
                    : Maybe<U>.None;

        public Maybe<U> Map<U>(Func<T, U> map) => HasValue
                    ? Maybe.Some(map(Value))
                    : Maybe<U>.None;
    }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value) => Maybe<T>.Some(value);
    }
}
