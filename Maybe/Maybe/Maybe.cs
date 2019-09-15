using System.Collections.Generic;
using System.Linq;

namespace System
{
    //Source from: https://www.pluralsight.com/tech-blog/maybe
    public struct Maybe<T>
    {
        private readonly IEnumerable<T> values;

        public bool HasValue => values != null && values.Any();

        public static implicit operator Maybe<T>(T value) => Some(value);

        public static Maybe<T> Some(T value) => value == null
                ? throw new ArgumentNullException($"Cannot add a null value for a reference type like {typeof(T).Name}")
                : new Maybe<T>(new[] { value });

        public static Maybe<T> None => new Maybe<T>(new T[0]);

        public T Value => HasValue
                ? values.Single()
                : throw new Exception($"Maybe of type {typeof(T).Name} does not have a value");

        /// Source from: https://mikhail.io/2016/01/monads-explained-in-csharp/
        /// https://mikhail.io/2018/07/monads-explained-in-csharp-again/
        public Maybe(T value) => values = Enumerable.Repeat(value, 1);

        private Maybe(IEnumerable<T> values) => this.values = values;

        public T ValueOrDefault(T @default) => !HasValue
                ? @default
                : values.Single();

        public T ValueOrThrow(Exception ex) => HasValue
                ? Value
                : throw ex;

        //Handle the cases where there is some value or there is none:
        public U Case<U>(Func<T, U> some, Func<U> none) => HasValue
                ? some(Value)
                : none();

        public Maybe<T> Case(Action<T> some, Action none)
        {
            if (HasValue)
                some(Value);
            else
                none();
            return this;
        }

        /// <summary>
        /// Performing an action if there is a value:
        /// var maybeAccount = repository.Find(accountId);
        /// maybeAccount.IfSome(account =>
        /// {
        ///     account.LastUpdated = DateTimeOffset.UtcNow;
        ///     repository.Save(account);
        /// });
        /// </summary>
        public Maybe<T> IfSome(Action<T> some)
        {
            if (HasValue)
            {
                some(Value);
            }

            return this;
        }

        /// <summary>
        /// Source from : https://mikhail.io/2018/07/monads-explained-in-csharp-again/
        /// Sample Usage:
        /// Maybe<Shipper> shipperOfLastOrderOnCurrentAddress =
        ///     repo.GetCustomer(customerId)
        ///     .Bind(c => c.Address)
        ///     .Bind(a => repo.GetAddress(a.Id))
        ///     .Bind(a => a.LastOrder)
        ///     .Bind(lo => repo.GetOrder(lo.Id))
        ///     .Bind(o => o.Shipper);
        /// </summary>
        public Maybe<U> Bind<U>(Func<T, Maybe<U>> func) where U : class
        {
            var value = values.SingleOrDefault();
            return value != null ? func(value) : new Maybe<U>();
        }

        /// <summary>
        /// Map a maybe to another type:
        /// Maybe<string> maybeFirstName = maybeAccount.Map(account => account.FirstName);
        /// Maybe<IList<string>> emails = maybeAccount.Map(account => repository.GetEmailAddresses(account));
        /// </summary>
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