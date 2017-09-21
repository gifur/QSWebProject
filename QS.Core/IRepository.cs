using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using QS.Core.Specification.Contract;

namespace QS.Core
{
    /// <summary/>
    /// 定义仓储模型中的数据标准操作
    /// 工作单元
    /// 对泛型类型的支持
    /// <remarks/>
    /// <typeparam name="TEntity">Type of entity for this repository </typeparam>
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(TEntity item);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(TEntity item);

        /// <summary>
        /// Set item as modified
        /// </summary>
        /// <param name="item">Item to modify</param>
        void Modify(TEntity item);

        /// <summary>
        ///Track entity into this repository, really in UnitOfWork. 
        ///In EF this can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void TrackItem(TEntity item);

        /// <summary>
        /// Sets modified entity into the repository. 
        /// When calling Commit() method in UnitOfWork 
        /// these changes will be saved into the storage
        /// </summary>
        /// <param name="persisted">The persisted item</param>
        /// <param name="current">The current item</param>
        void Merge(TEntity persisted, TEntity current);

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">Entity key value</param>
        /// <returns></returns>
        TEntity Get(int id);

        TEntity Get(Guid id);

        TEntity Get(Int64 id);

        /// <summary>
        /// 获取当前实体的查询数据集
        /// </summary>
        //IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// 获取存储库中类型为TEntity的元素
        /// </summary>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 获取存储库中满足条件的类型为TEntity的元素
        /// Specification <paramref name="specification"/>
        /// </summary>
        /// <param name="specification">Specification that result meet</param>
        /// <returns></returns>
        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);

        IEnumerable<TEntity> ExecuteQuery(string sqlQuery);
        int ExecuteCommand(string sqlQuery);

        IEnumerable<TEntity> GetPagedWithFilter<TKProperty>(
            Expression<Func<TEntity, bool>> filter,
            int pageIndex, int pageCount, out int  count,
            Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending);

        /// <summary>
        /// 获取存储库中类型为TEntity的元素
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageCount">每页的数量</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">是否排序</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageCount, 
            Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageCount, out int count,
            Expression<Func<TEntity, TProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetPaged<TProperty, TPropertySec>(int pageIndex, int pageCount,
            Expression<Func<TEntity, TProperty>> orderByExpression, Expression<Func<TEntity, TPropertySec>> thenOrderByExpression, 
            bool ascending, out int count);

        IEnumerable<TEntity> GetPagedWithFilter<TProperty, TPropertySec>(
            Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount,
            Expression<Func<TEntity, TProperty>> orderByExpression, 
            Expression<Func<TEntity, TPropertySec>> thenOrderByExpression,
            bool ascending, out int count);

        int Count(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAllWithOrder<TKProperty>(Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending = false);

        /// <summary>
        /// 获取存储库中类型为TEntity的元素
        /// </summary>
        /// <param name="filter">筛选的条件</param>
        /// <returns>选中元素的集合</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, out int count);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    }
}
