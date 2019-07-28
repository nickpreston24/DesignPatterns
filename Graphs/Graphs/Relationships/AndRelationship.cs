using System;
using System.Linq.Expressions;

namespace Graphs
{
    //internal class AndRelationship<T> : Relationship<T>
    //    where T : INode
    //{
    //    private readonly Relationship<T> left;
    //    private readonly Relationship<T> right;

    //    public AndRelationship(Relationship<T> left, Relationship<T> right)
    //    {
    //        this.right = right;
    //        this.left = left;
    //    }

    //    public override Expression<Func<T, T, bool>> Condition()
    //    {
    //        var leftExpression = left.Condition();
    //        var rightExpression = right.Condition();

    //        var paramExpr = Expression.Parameter(typeof(T));
    //        var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
    //        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
    //        var finalExpr = Expression.Lambda<Func<T, T, bool>>(exprBody, paramExpr);

    //        return finalExpr;
    //    }
    //}

    //internal class AndRelationship<T, O> : Relationship<T, O>
    //    where O : INode
    //    where T : INode
    //{
    //    private readonly Relationship<T, O> left;
    //    private readonly Relationship<T, O> right;

    //    public AndRelationship(Relationship<T, O> left, Relationship<T, O> right)
    //    {
    //        this.right = right;
    //        this.left = left;
    //    }

    //    public override Expression<Func<T, O, bool>> ToExpression()
    //    {
    //        var leftExpression = left.ToExpression();
    //        var rightExpression = right.ToExpression();

    //        var paramExpr = Expression.Parameter(typeof(T));
    //        var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
    //        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
    //        var finalExpr = Expression.Lambda<Func<T, O, bool>>(exprBody, paramExpr);

    //        return finalExpr;
    //    }
    //}
}