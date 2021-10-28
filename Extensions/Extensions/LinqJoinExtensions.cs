using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
	/// <summary>
	/// Linq Join Extensions
	/// Source: https://www.codeproject.com/articles/488643/linq-extended-joins
	/// GitHub:m https://github.com/moraleslarios/MoralesLarios.Linq/blob/master/MoralesLarios.Linq/MLEnumerable.cs
	/// </summary>
	public static class LinqJoinExtensions
	{
		/// <summary>
		/// Left Join:
		/// </summary>
		public static IEnumerable<TResult>
		LeftJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
													IEnumerable<TInner> inner,
													Func<TSource, TKey> pk,
													Func<TInner, TKey> fk,
													Func<TSource, TInner, TResult> result)
		{
			IEnumerable<TResult> _result = Enumerable.Empty<TResult>();
			_result = from s in source
					  join i in inner
					  on pk(s) equals fk(i) into joinData
					  from left in joinData.DefaultIfEmpty()
					  select result(s, left);
			return _result;
		}

		/// <summary>
		/// Right Join:
		/// </summary>
		public static IEnumerable<TResult>
		RightJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
												  IEnumerable<TInner> inner,
												  Func<TSource, TKey> pk,
												  Func<TInner, TKey> fk,
												  Func<TSource, TInner, TResult> result)
		{
			IEnumerable<TResult> _result = Enumerable.Empty<TResult>();
			_result = from i in inner
					  join s in source
					  on fk(i) equals pk(s) into joinData
					  from right in joinData.DefaultIfEmpty()
					  select result(right, i);
			return _result;
		}

		/// <summary>
		/// Full Outer Join:
		/// </summary>
		public static IEnumerable<TResult>
		FullOuterJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
														  IEnumerable<TInner> inner,
														  Func<TSource, TKey> pk,
														  Func<TInner, TKey> fk,
														  Func<TSource, TInner, TResult> result)
		{
			List<TResult> left = source.LeftJoin(inner, pk, fk, result).ToList();
			List<TResult> right = source.RightJoin(inner, pk, fk, result).ToList();
			return left.Union(right);
		}

		/// <summary>
		/// Left Excluding Join:
		/// </summary>
		public static IEnumerable<TResult>
		LeftExcludingJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
														  IEnumerable<TInner> inner,
														  Func<TSource, TKey> pk,
														  Func<TInner, TKey> fk,
														  Func<TSource, TInner, TResult> result)
		{
			IEnumerable<TResult> _result = Enumerable.Empty<TResult>();
			_result = from s in source
					  join i in inner
					  on pk(s) equals fk(i) into joinData
					  from left in joinData.DefaultIfEmpty()
					  where left == null
					  select result(s, left);
			return _result;
		}

		/// <summary>
		/// Right Excluding Join:
		/// </summary>
		public static IEnumerable<TResult>
		RightExcludingJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
														IEnumerable<TInner> inner,
														Func<TSource, TKey> pk,
														Func<TInner, TKey> fk,
														Func<TSource, TInner, TResult> result)
		{
			IEnumerable<TResult> _result = Enumerable.Empty<TResult>();
			_result = from i in inner
					  join s in source
					  on fk(i) equals pk(s) into joinData
					  from right in joinData.DefaultIfEmpty()
					  where right == null
					  select result(right, i);
			return _result;
		}

		/// <summary>
		/// Full Outer Excluding Join:
		/// </summary>
		public static IEnumerable<TResult>
		FullOuterExcludingJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
													  IEnumerable<TInner> inner,
													  Func<TSource, TKey> pk,
													  Func<TInner, TKey> fk,
													  Func<TSource, TInner, TResult> result)
		{
			List<TResult> left = source.LeftExcludingJoin(inner, pk, fk, result).ToList();
			List<TResult> right = source.RightExcludingJoin(inner, pk, fk, result).ToList();
			return left.Union(right);
		}
	}
}