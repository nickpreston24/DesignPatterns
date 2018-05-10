using System;

namespace Common
{
    //
    /// The Following is a series of Funcs for the purpose of handling functional programming in C#
    ////
    public static partial class CommonExtensions
    {
        public static Func<T> Cache<T>(this Func<T> function, int cacheInterval)
        {
            var cachedValue = function();
            var timeCached = DateTime.Now;

            Func<T> cachedFunc = () =>
            {
                if ((DateTime.Now - timeCached).Seconds >= cacheInterval)
                {
                    timeCached = DateTime.Now;
                    cachedValue = function();
                }

                return cachedValue;
            };

            return cachedFunc;
        }

        public static Func<T, bool> Not<T>(this Func<T, bool> predicate) { return a => !predicate(a); }

        public static Func<T, bool> And<T>(this Func<T, bool> left, Func<T, bool> right) { return a => left(a) && right(a); }

        public static Func<T, bool> Or<T>(this Func<T, bool> left, Func<T, bool> right) { return a => left(a) || right(a); }

        // Y - Combinator        
        private delegate Func<A, R> Recursive<A, R>(Recursive<A, R> r);

        public static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
        {
            Recursive<A, R> rec = r => a => f(r(r))(a);
            return rec(rec);
        }

        //
        /// Currying Funcs
        ////
        public static Func<T1, T2, TResult> UnCurry<T1, T2, TResult>(this Func<T1, Func<T2, TResult>> func)
            => (x1, x2) => func(x1)(x2);
        public static Func<T1, T2, T4> Compose<T1, T2, T3, T4>(this Func<T1, T2, T3> f, Func<T3, T4> g)
            => (x, y) => g(f(x, y));
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
            => x1 => x2 => func(x1, x2);
        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
            => x1 => x2 => x3 => func(x1, x2, x3);
        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
            => x1 => x2 => x3 => x4 => func(x1, x2, x3, x4);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> Curry<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => func(x1, x2, x3, x4, x5);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TResult>>>>>> Curry<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => func(x1, x2, x3, x4, x5, x6);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => func(x1, x2, x3, x4, x5, x6, x7);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => func(x1, x2, x3, x4, x5, x6, x7, x8);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, TResult>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, TResult>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, TResult>>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => x11 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, TResult>>>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => x11 => x12 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, TResult>>>>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => x11 => x12 => x13 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, TResult>>>>>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => x11 => x12 => x13 => x14 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, TResult>>>>>>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => x11 => x12 => x13 => x14 => x15 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15);
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, Func<T11, Func<T12, Func<T13, Func<T14, Func<T15, Func<T16, TResult>>>>>>>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func)
            => x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => x11 => x12 => x13 => x14 => x15 => x16 => func(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15, x16);
    }
}
