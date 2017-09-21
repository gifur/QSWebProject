using System;
using System.Linq;
using System.Linq.Expressions;

namespace QS.Core.Specification.Common
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// 将两则表达式合并成一条表达式
        /// </summary>
        /// <typeparam name="T">参数表达式的类型</typeparam>
        /// <param name="first">表达式实例</param>
        /// <param name="second">合并的表达式</param>
        /// <param name="merge">合并的方法</param>
        /// <returns></returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            // 创建参数映射（从第二个参数到第一个参数）
            var map = first.Parameters.Select((f, i) => new {f, s = second.Parameters[i]})
                .ToDictionary(p => p.s, p => p.f);

            //通过第一条表达式的参数替换第二条lambda表达式的参数
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// And 运算
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="first">与操作的右边参数表达式</param>
        /// <param name="second">与操作的左边参数表达式</param>
        /// <returns>新的与表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// 或操作
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="first">或操作的右边参数表达式</param>
        /// <param name="second">与操作的左边参数表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }
}
