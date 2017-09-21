using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using QS.Common.Logging;
using QS.Core;
using QS.Core.Specification.Contract;
using QS.DAL.Contract;

namespace QS.DAL
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Members

        IQueryableUnitOfWork _UnitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// 创建仓储实例
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork");

            _UnitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        /// <summary>
        /// 
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(TEntity item)
        {

            if (item != (TEntity)null)
                GetSet().Add(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotAddNullEntity, typeof(TEntity).ToString());
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Remove(TEntity item)
        {
            if (item != (TEntity)null)
            {
                //attach item if not exist
                _UnitOfWork.Attach(item);

                //将给定实体标记成“已删除”，这样一来在进行SaveChange()时，将从数据库中删除该实体
                GetSet().Remove(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void TrackItem(TEntity item)
        {
            if (item != (TEntity)null)
                _UnitOfWork.Attach<TEntity>(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Modify(TEntity item)
        {
            if (item != (TEntity)null)
                _UnitOfWork.SetModified(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo(Message.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// 根据Id获取某值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity Get(int id)
        {
            return id != 0 ? GetSet().Find(id) : null;
        }

        public virtual TEntity Get(Guid id)
        {
            return GetSet().Find(id);
        }
        public virtual TEntity Get(Int64 id)
        {
            return id != 0 ? GetSet().Find(id) : null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet().AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        {
            return GetSet().Where(specification.SatisfiedBy());
        }

        public virtual IEnumerable<TEntity> ExecuteQuery(string sqlQuery)
        {
            return _UnitOfWork.ExecuteQuery<TEntity>(sqlQuery);
        }
        public virtual int ExecuteCommand(string sqlQuery)
        {
            return _UnitOfWork.ExecuteCommand(sqlQuery);
        }


        public virtual IEnumerable<TEntity> GetPagedWithFilter<TKProperty>(
            Expression<Func<TEntity, bool>> filter,
            int pageIndex, int pageCount, out int count,
            Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending)
        {
            var set = GetFiltered(filter).AsQueryable();
            count = set.Count();
            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }

            return set.OrderByDescending(orderByExpression)
                .Skip(pageCount * pageIndex)
                .Take(pageCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKProperty"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetPaged<TKProperty>(
            int pageIndex, int pageCount, 
            Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }

            return set.OrderByDescending(orderByExpression)
                .Skip(pageCount * pageIndex)
                .Take(pageCount);
        }

        public IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageCount, out int count, Expression<Func<TEntity, TProperty>> orderByExpression,
            bool @ascending)
        {
            var set = GetSet();
            count = set.Count();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                           .Skip(pageCount * (pageIndex - 1))
                          .Take(pageCount);
            }

            return set.OrderByDescending(orderByExpression)
                 .Skip(pageCount * (pageIndex - 1))
                .Take(pageCount);
        }

        public IEnumerable<TEntity> GetPaged<TProperty, TPropertySec>(int pageIndex, int pageCount, Expression<Func<TEntity, TProperty>> orderByExpression,
            Expression<Func<TEntity, TPropertySec>> thenOrderByExpression, bool ascending, out int count)
        {
            var set = GetSet();
            count = set.Count();

            if (ascending)
            {
                return set.OrderBy(orderByExpression).ThenByDescending(thenOrderByExpression)
                          .Skip(pageCount * (pageIndex - 1))
                          .Take(pageCount);
            }

            return set.OrderByDescending(orderByExpression).ThenByDescending(thenOrderByExpression)
                .Skip(pageCount * (pageIndex-1))
                .Take(pageCount);
        }

        public IEnumerable<TEntity> GetPagedWithFilter<TProperty, TPropertySec>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount,
            Expression<Func<TEntity, TProperty>> orderByExpression, Expression<Func<TEntity, TPropertySec>> thenOrderByExpression, bool @ascending, out int count)
        {
            var set = GetFiltered(filter).AsQueryable();
            count = set.Count();

            if (ascending)
            {
                return set.OrderBy(orderByExpression).ThenByDescending(thenOrderByExpression)
                          .Skip(pageCount * (pageIndex - 1))
                          .Take(pageCount);
            }

            return set.OrderByDescending(orderByExpression).ThenByDescending(thenOrderByExpression)
                .Skip(pageCount * (pageIndex - 1))
                .Take(pageCount);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().Where(predicate).Count();
        }

        public virtual IEnumerable<TEntity> GetAllWithOrder<TKProperty>( Expression<Func<TEntity, TKProperty>> orderByExpression, bool ascending = false)
        {
            var set = GetSet();

            if (ascending)
            {
                return set.OrderBy(orderByExpression);
            }
            else
            {
                return set.OrderByDescending(orderByExpression);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Where(filter);
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, out int count)
        {
            var temp = GetSet().Where(filter);
            count = temp.Count();
            return temp;
        }

        /// <summary>
        /// 取序列中满足条件的第一个元素，如果没有元素满足条件，则返回默认值
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns>对于可以为null的对象，默认值为null，对于不能为null的对象，如int，默认值为0</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().FirstOrDefault(predicate);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="persisted"></param>
        /// <param name="current"></param>
        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _UnitOfWork.ApplyCurrentValues(persisted, current);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }

        #endregion

        #region Private Methods

        IDbSet<TEntity> GetSet()
        {
            return _UnitOfWork.CreateSet<TEntity>();
        }
        #endregion

    }
}
