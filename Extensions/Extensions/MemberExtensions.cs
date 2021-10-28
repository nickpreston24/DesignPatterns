using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Extensions
{
	public static class MemberExtensions
	{
		private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
		private static readonly string invalidExpressionMessage = "Invalid expression.";

		public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression) => GetMemberName(expression.Body);

		public static string GetMemberName<T>(this Expression<Func<T, object>> expression) => GetMemberName(default, expression);

		public static IEnumerable<string> GetMemberNames<T>(this T instance, params Expression<Func<T, object>>[] expressions)
		{
			List<string> memberNames = new List<string>();
			foreach (Expression<Func<T, object>> expression in expressions)
			{
				memberNames.Add(GetMemberName(expression.Body));
			}
			return memberNames;
		}

		public static IEnumerable<string> GetMemberNames<T>(params Expression<Func<T, object>>[] expressions) => GetMemberNames(default, expressions);

		public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression) => GetMemberName(expression.Body);

		public static string GetMemberName<T>(this Expression<Action<T>> expression) => GetMemberName(default, expression);

		private static string GetMemberName(Expression expression)
		{
			if (expression == null)
			{
				throw new ArgumentException(expressionCannotBeNullMessage);
			}
			if (expression is MemberExpression)
			{
				// Reference type property or field
				MemberExpression memberExpression = (MemberExpression)expression;
				return memberExpression.Member.Name;
			}
			if (expression is MethodCallExpression)
			{
				// Reference type method
				MethodCallExpression methodCallExpression = (MethodCallExpression)expression;
				return methodCallExpression.Method.Name;
			}
			if (expression is UnaryExpression)
			{
				// Property, field of method returning value type
				UnaryExpression unaryExpression = (UnaryExpression)expression;
				return GetMemberName(unaryExpression);
			}
			throw new ArgumentException(invalidExpressionMessage);
		}

		private static string GetMemberName(UnaryExpression unaryExpression)
		{
			if (unaryExpression.Operand is MethodCallExpression)
			{
				MethodCallExpression methodExpression = (MethodCallExpression)unaryExpression.Operand;
				return methodExpression.Method.Name;
			}
			return ((MemberExpression)unaryExpression.Operand).Member.Name;
		}
	}
}
