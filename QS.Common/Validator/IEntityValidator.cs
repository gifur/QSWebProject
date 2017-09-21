using System;
using System.Collections.Generic;

namespace QS.Common.Validator
{
    /// <summary>
    /// The entity validator base contract
    /// </summary>
    public interface IEntityValidator
    {
        /// <summary>
        /// 进行验证，并返回实体是否可用的信息
        /// </summary>
        /// <typeparam name="TEntity">验证的项的类型</typeparam>
        /// <param name="item">验证的指定项</param>
        /// <returns>如果实体对象可用的话则返回真</returns>
        bool IsValid<TEntity>(TEntity item)
            where TEntity : class;

        /// <summary>
        /// 返回错误的集合如果实体状态为不可用时
        /// </summary>
        /// <typeparam name="TEntity">验证的项的类型</typeparam>
        /// <param name="item">验证错误的项</param>
        /// <returns>错误信息的集合</returns>
        IEnumerable<String> GetInvalidMessages<TEntity>(TEntity item)
            where TEntity : class;
    }
}
