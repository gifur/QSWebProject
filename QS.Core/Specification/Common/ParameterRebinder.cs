using System.Collections.Generic;
using System.Linq.Expressions;

namespace QS.Core.Specification.Common
{
    /// <summary>
    /// Helper for rebinder parameters without use Invoke method in expressions 
    /// ( this methods is not supported in all linq query providers, 
    /// for example in Linq2Entities is not supported)
    /// </summary>
    public sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        /// <param name="map">映射规范</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// 通过映射信息替换表达式的参数
        /// </summary>
        /// <param name="map">映射信息</param>
        /// <param name="exp">替换参数的表达式</param>
        /// <returns>返回含替换参数的表达式</returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// 访问模式方法
        /// </summary>
        /// <param name="p">参数表达式</param>
        /// <returns>新的已访问的表达式</returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
