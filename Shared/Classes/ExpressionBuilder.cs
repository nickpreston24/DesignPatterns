using System.Collections.Generic;

namespace System.Linq.Expressions
{
    public static class ExpressionBuilder
    {
        public static LambdaExpression Build<T>(string condition, params object[] values)
        {
            try
            {
                return Dynamic.DynamicExpression.ParseLambda(typeof(T), typeof(bool), condition, values);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool CheckCondition<T>(T instance, string condition, params object[] values)
            where T : class
        {
            if (instance == null)
            {
                throw new NullReferenceException($"{typeof(T).Name} instance");
            }

            return (bool)Build<T>(condition, values).Compile().DynamicInvoke(instance);
        }

        public static IEnumerable<T> CheckCondition<T>(IEnumerable<T> collection, string conditions, params object[] values)
            where T : class
        {
            try
            {
                var lambda = Build<T>(conditions, values);
                return ExpressionExtensions.Where(collection, lambda);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static partial class ExpressionExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> collection, LambdaExpression expression)
            where T : class
        {
            try
            {
                var compiledLambda = expression.Compile();
                return collection.OfType<T>().Where(x => (bool)compiledLambda.DynamicInvoke(x));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> collection, string condition, params object[] values)
            where T : class
        {
            try
            {
                return collection.OfType<T>().Where(ExpressionBuilder.Build<T>(condition, values));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> All<T>(this IEnumerable<T> collection, IEnumerable<string> conditions, params object[] values)
           where T : class
        {
            try
            {
                return collection.OfType<T>().Where(instance => conditions.All(condition => ExpressionBuilder.CheckCondition(instance, condition, values)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> Any<T>(this IEnumerable<T> collection, IEnumerable<string> conditions, params object[] values)
           where T : class
        {
            try
            {
                return collection.OfType<T>().Where(instance => conditions.Any(condition => ExpressionBuilder.CheckCondition(instance, condition, values)));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}