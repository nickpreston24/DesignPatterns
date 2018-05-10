using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common
{
    public static partial class CommonExtensions
    {
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> sequence, int startIndex, int count)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            // optimization for anything implementing IList<T>
            return !(sequence is IList<T> list)
                 ? sequence.Skip(startIndex).Take(count)
                 : _(count); IEnumerable<T> _(int countdown)
            {
                var listCount = list.Count;
                var index = startIndex;
                while (index < listCount && countdown-- > 0)
                {
                    yield return list[index++];
                }
            }
        }

        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source is ICollection<TSource> col
                ? col.Slice(Math.Max(0, col.Count - count), int.MaxValue)
                : _(); IEnumerable<TSource> _()
            {
                if (count <= 0)
                {
                    yield break;
                }

                var q = new Queue<TSource>(count);

                foreach (var item in source)
                {
                    if (q.Count == count)
                    {
                        q.Dequeue();
                    }

                    q.Enqueue(item);
                }

                foreach (var item in q)
                {
                    yield return item;
                }
            }
        }

        //From: https://github.com/morelinq/MoreLINQ/blob/master/MoreLinq/MaxBy.cs#L43
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            comparer = comparer ?? Comparer<TKey>.Default;
            return ExtremumBy(source, selector, (x, y) => comparer.Compare(x, y));
        }

        static TSource ExtremumBy<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> selector, Func<TKey, TKey, int> comparer)
        {
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");

                var extremum = sourceIterator.Current;
                var key = selector(extremum);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer(candidateProjected, key) > 0)
                    {
                        extremum = candidate;
                        key = candidateProjected;
                    }
                }

                return extremum;
            }
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
           Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            comparer = comparer ?? Comparer<TKey>.Default;
            return ExtremumBy(source, selector, (x, y) => -Math.Sign(comparer.Compare(x, y)));
        }

        public static DataTable ToDatatable<T>(this IEnumerable<T> collection)
        {
            var table = new DataTable();

            if (collection == null || !collection.Any())
            {
                return table;
            }

            var properties = propertyCache[typeof(T)];

            if (properties.Length == 0)
            {
                return table;
            }

            var values = new object[properties.Length];

            try
            {
                foreach (var item in collection ?? Enumerable.Empty<T>())
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = properties[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }

        public static IEnumerable<T> AsEmpty<T>(this IEnumerable<T> source) => Enumerable.Empty<T>();

        public static IEnumerable<T> Interweave<T>(params T[] inputs) => Interweave<T>(inputs);

        public static IEnumerable<T> Interweave<T>(this IEnumerable<IEnumerable<T>> inputs)
        {
            var enumerators = new List<IEnumerator<T>>();
            try
            {
                foreach (var input in inputs)
                {
                    enumerators.Add(input.GetEnumerator());
                }

                while (true)
                {
                    enumerators.RemoveAll(enumerator =>
                    {
                        if (enumerator.MoveNext())
                        {
                            return false;
                        }

                        enumerator.Dispose();
                        return true;
                    });

                    if (enumerators.Count == 0)
                    {
                        yield break;
                    }

                    foreach (var enumerator in enumerators)
                    {
                        yield return enumerator.Current;
                    }
                }
            }
            finally
            {
                if (enumerators != null)
                {
                    foreach (var e in enumerators)
                    {
                        e.Dispose();
                    }
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> query, int batchSize)
        {
            using (var enumerator = query.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.EnumerateSome(batchSize);
                }
            }
        }

        internal static IEnumerable<T> EnumerateSome<T>(this IEnumerator<T> enumerator, int count)
        {
            var list = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                list.Add(enumerator.Current);
                if (!enumerator.MoveNext())
                {
                    break;
                }
            }

            foreach (var item in list)
            {
                yield return item;
            }
        }

        //public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxBatchSize)
        //{
        //    return items.Select((item, index) => new { item, index })
        //        .GroupBy(pairs => pairs.index / maxBatchSize)
        //        .Select(mapped => mapped.Select(pair => pair.item));
        //}

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || collection.Count() == 0;

        public static string ToCsv<T>(this IEnumerable<T> items)
           where T : class
        {
            var csvBuilder = new StringBuilder();
            var properties = propertyCache[typeof(T)];

            foreach (var item in items ?? Enumerable.Empty<T>())
            {
                string line = string.Join(",", properties
                    .Select(property =>
                        property.GetValue(item, null)
                        .ToCsvValue()).ToArray());

                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }

        private static string ToCsvValue<T>(this T item)
        {
            if (item == null)
            {
                return "\"\"";
            }

            if (item is string)
            {
                return string.Format("\"{0}\"", item.ToString().Replace("\"", "\\\""));
            }

            if (double.TryParse(item.ToString(), out double dummy))
            {
                return string.Format("{0}", item);
            }

            return string.Format("\"{0}\"", item);
        }

        public static void RunActionOn<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null)
            {
                return;
            }

            var cached = collection;

            foreach (var item in cached ?? Enumerable.Empty<T>())
            {
                action(item);
            }
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> collection, int count) => collection.OrderBy(g => Guid.NewGuid()).Take(count);

        public static T TakeFirstRandom<T>(this IEnumerable<T> collection) => collection.OrderBy(c => Guid.NewGuid()).FirstOrDefault();

        public static IEnumerable<T> MoveUp<T>(this IEnumerable<T> enumerable, int itemIndex)
        {
            int i = 0;
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                i++;
                if (itemIndex.Equals(i))
                {
                    var previous = enumerator.Current;
                    if (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                    yield return previous;
                    break;
                }
                yield return enumerator.Current;
            }
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<T> GetItemsWhere<T>(this IEnumerable<T> collection, LambdaExpression condition)
        {
            try
            {
                var compiledLambda = condition.Compile();
                return collection.Where(x => (bool)compiledLambda.DynamicInvoke(x));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> GetItemsWhere<T>(this IEnumerable<T> collection, Expression<Func<T, bool>> expression, int numDesired = 1)
        {
            try
            {
                Func<T, bool> funcWhere = expression.Compile();
                return collection.Where(funcWhere).Take(numDesired);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> GetItemsWhere<T>(this IEnumerable<T> collection, Expression<Func<T, bool>> where)
        {
            try
            {
                Func<T, bool> funcWhere = where.Compile();
                return collection.Where(funcWhere);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> GetRandomItemsWhere<T>(this IEnumerable<T> collection, Expression<Func<T, bool>> whereclause, int count)
        {
            try
            {
                Func<T, bool> funcWhere = whereclause.Compile();
                return collection.Where(funcWhere).OrderBy(c => Guid.NewGuid()).Take(count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> GetRandomItems<T>(this IEnumerable<T> collection, int count)
        {
            try
            {
                return collection.OrderBy(c => Guid.NewGuid()).Take(count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        * The following are Linq Join Extensions
        * Source: https://www.codeproject.com/articles/488643/linq-extended-joins
        */

        public static IEnumerable<TResult> LeftJoin<TSource, TInner, TKey, TResult>
            (this IEnumerable<TSource> sourceCollection, IEnumerable<TInner> innerCollection,
                  Func<TSource, TKey> pk, Func<TInner, TKey> fk,
                  Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();
            _result = from s in sourceCollection
                      join i in innerCollection
                      on pk(s) equals fk(i) into joinData
                      from left in joinData.DefaultIfEmpty()
                      select result(s, left);
            return _result;
        }

        public static IEnumerable<TResult>
        RightJoin<TSource, TInner, TKey, TResult>
            (this IEnumerable<TSource> source, IEnumerable<TInner> inner,
                  Func<TSource, TKey> pk, Func<TInner, TKey> fk,
                  Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();
            _result = from i in inner
                      join s in source
                      on fk(i) equals pk(s) into joinData
                      from right in joinData.DefaultIfEmpty()
                      select result(right, i);
            return _result;
        }

        public static IEnumerable<TResult> FullOuterJoin<TSource, TInner, TKey, TResult>
            (this IEnumerable<TSource> sourceCollection, IEnumerable<TInner> inner,
                  Func<TSource, TKey> pk, Func<TInner, TKey> fk,
                  Func<TSource, TInner, TResult> result)
        {
            var left = sourceCollection.LeftJoin(inner, pk, fk, result).ToList();
            var right = sourceCollection.RightJoin(inner, pk, fk, result).ToList();
            return left.Union(right);
        }

        public static IEnumerable<TResult> LeftExcludingJoin<TSource, TInner, TKey, TResult>
            (this IEnumerable<TSource> source, IEnumerable<TInner> inner,
                  Func<TSource, TKey> pk, Func<TInner, TKey> fk,
                  Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();
            _result = from s in source
                      join i in inner
                      on pk(s) equals fk(i) into joinData
                      from left in joinData.DefaultIfEmpty()
                      where left == null
                      select result(s, left);
            return _result;
        }

        public static IEnumerable<TResult> RightExcludingJoin<TSource, TInner, TKey, TResult>
            (this IEnumerable<TSource> source, IEnumerable<TInner> inner,
                  Func<TSource, TKey> pk, Func<TInner, TKey> fk,
                  Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();
            _result = from i in inner
                      join s in source
                      on fk(i) equals pk(s) into joinData
                      from right in joinData.DefaultIfEmpty()
                      where right == null
                      select result(right, i);
            return _result;
        }

        public static IEnumerable<TResult> FullOuterExcludingJoin<TSource, TInner, TKey, TResult>
            (this IEnumerable<TSource> sourceCollection, IEnumerable<TInner> innerCollection,
                 Func<TSource, TKey> pk, Func<TInner, TKey> fk,
                 Func<TSource, TInner, TResult> result)
        {
            var left = sourceCollection.LeftExcludingJoin(innerCollection, pk, fk, result).ToList();
            var right = sourceCollection.RightExcludingJoin(innerCollection, pk, fk, result).ToList();
            return left.Union(right);
        }
    }
}
