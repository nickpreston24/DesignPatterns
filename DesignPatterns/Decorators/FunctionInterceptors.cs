using System;
using System.Collections.Generic;

/* Keep this namespace as is, refers to src from: 
   https://www.codeproject.com/Articles/1104555/Function-Decorator-Pattern-Reanimation-of-Function
*/
namespace Decorators.FunctionInterceptors
{
    public static partial class FunctionInterceptors
    {
        /// <summary>
        /// Retry a function
        /// Can be used to Intercept bad result and try it again for a positive.
        /// </summary>
        public static Func<A, Result> Retry<A, Result>(this Func<A, Result> repeatingAction, int maxRetries = 3)
        {
            return (arg) =>
            {
                int tried = 0;
                do
                {
                    try
                    {
                        return repeatingAction(arg);
                    }
                    catch (Exception)
                    {
                        if (++tried > maxRetries)
                            throw new Exception($"Exceeded maximum of {maxRetries} tries!");
                    }
                } while (true);
            };
        }

        /// <summary>
        /// Get a result or cache its value
        /// Use with <seealso cref="Retry{A,Result}" 
        /// to get free caching of last tried values after failing an operation/>
        /// </summary>
        public static Func<A, Result> GetOrCache<A, Result, Cache>(this Func<A, Result> action, Cache cache)
            where Cache : class, IDictionary<A, Result>
        {
            return (arg) =>
            {
                if (cache.TryGetValue(arg, out var value))
                    return value;

                value = action(arg);
                cache.Add(arg, value);
                return action(arg);
            };
        }

        #region My Interceptor Attempts
        
        /// <summary>
        /// Should chain off of a single arg func or method, perform a side-effect and return the Result
        /// encapsulated in another function (partial?) 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <returns></returns>
        public static Func<A, Result> With<A, Result>(this Func<A, Result> action) =>
            arg => action(arg);

        // public static Func<A, Result> UnCurry<A, Result>(this Func<A, Func<A, Result>> curriedFunc) => 
        //     arg => curriedFunc(arg)();

        // class SampleCurrying
        // {
        //     
        // }

        #endregion My Interceptor Attempts

    }
}