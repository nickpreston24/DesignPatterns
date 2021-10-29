using System;
using System.Linq.Expressions;

namespace Common.Extensions
{
    public static partial class Extensions
    {
        //https://stackoverflow.com/questions/654153/c-sharp-how-to-convert-an-expressionfuncsometype-to-an-expressionfuncother
        public static Func<T, object> ExpressionConversion<T, U>(this Expression<Func<T, U>> expression)
        {
            Expression<Func<T, object>> g = obj => expression.Compile().Invoke(obj);
            return g.Compile();
        }

        /// The Following is a series of Expressions for the purpose of handling functional programming in C#
        /// and for creating Expression Trees, which are used in this case to evaluate logical constructs (like predicates)
        /// I will add more as I learn more.
        ////
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicate)
            => Expression.Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);

        //public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        //    => Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, right.WithParametersOf(left).Body), left.Parameters);
        //public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        //    => Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, right.WithParametersOf(left).Body), left.Parameters);
        //TODO: Implement XOR!
        //private static Expression<Func<TResult>> WithParametersOf<T, TResult>(this Expression<Func<T, TResult>> left, Expression<Func<T, TResult>> right) => new ReplaceParameterVisitor<Func<TResult>>(left.Parameters[0], right.Parameters[0]).Visit(left);
    }
}
