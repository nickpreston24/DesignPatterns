using System.Linq.Expressions;

namespace DesignPatterns.Specifications
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        readonly ParameterExpression parameter;

        protected override Expression VisitParameter(ParameterExpression node)
            => base.VisitParameter(parameter);

        internal ParameterReplacer(ParameterExpression parameter)
            => this.parameter = parameter;
    }
}