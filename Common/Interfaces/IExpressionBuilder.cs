using System.Linq.Expressions;

namespace Common
{
    //TODO:  Create a service class that builds conditions for Dynamic Expressions using operators and operands and given types.  This service must be applicable to any object whose properties can be stringified or have some value.
    public interface IExpressionBuilder
    {
        /// <summary>
        /// takes uncompiled condition(s) and converts to a LambdaExpression
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        LambdaExpression Build<T>(string condition);
    }
}
