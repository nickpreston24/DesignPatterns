using System;
using System.Linq.Expressions;

namespace DesignPatterns
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> Condition();

        public bool IsSatisfiedBy(T candidate)
        {
            var predicate = Condition().Compile();
            return predicate(candidate);
        }

        public Specification<T> And(Specification<T> specification)
            => new AndSpecification<T>(this, specification);

        public Specification<T> Or(Specification<T> specification)
            => new OrSpecification<T>(this, specification);
    }

    internal class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> left;
        private readonly Specification<T> right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            this.right = right;
            this.left = left;
        }

        public override Expression<Func<T, bool>> Condition()
        {
            var leftExpression = left.Condition();
            var rightExpression = right.Condition();

            ///Working advanced, but slow code

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);
            return (Expression<Func<T, bool>>)Expression.Lambda(Expression.AndAlso(leftExpression.Body, invokedExpression), leftExpression.Parameters);

            ///Original Git? code (Has the replacement error):

            //var paramExpr = Expression.Parameter(typeof(T));
            //var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            //exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            //var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            //return finalExpr;

            ///Article code:
            //BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            //return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());
        }
    }

    internal class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> left;
        private readonly Specification<T> right;

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            this.right = right;
            this.left = left;
        }

        public override Expression<Func<T, bool>> Condition()
        {
            var leftExpression = left.Condition();
            var rightExpression = right.Condition();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);
            return (Expression<Func<T, bool>>)Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression), leftExpression.Parameters);

            //var paramExpr = Expression.Parameter(typeof(T));
            //var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            //exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            //var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            //return finalExpr;
        }
    }

    internal class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> left;
        private readonly Specification<T> right;

        public NotSpecification(Specification<T> left, Specification<T> right)
        {
            this.right = right;
            this.left = left;
        }

        public override Expression<Func<T, bool>> Condition()
        {
            var leftExpression = left.Condition();
            var rightExpression = right.Condition();

            var invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);
            return (Expression<Func<T, bool>>)Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression), leftExpression.Parameters);

            //var paramExpr = Expression.Parameter(typeof(T));
            //var exprBody = Expression.Not(leftExpression.Body, rightExpression.Body);
            //exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            //var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            //return finalExpr;
        }
    }
}