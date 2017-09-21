using System.Data.Entity;
using QS.Core;

namespace QS.DAL.Contract
{
    /// <summary>
    /// The UnitOfWork contract for EF implementation
    /// <remarks>
    /// This contract extend IUnitOfWork for use with EF code
    /// </remarks>
    /// </summary>
    public interface IQueryableUnitOfWork
        : IUnitOfWork, ISql
    {
        /// <summary>
        /// Returns a IDbSet instance for access to entities of the given type in the context, 
        /// the ObjectStateManager, and the underlying store. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> CreateSet<T>() where T : class;

        /// <summary>
        /// Attach this item into "ObjectStateManager"
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="item">The item</param>
        void Attach<T>(T item) where T : class;

        /// <summary>
        /// 修改实体为“修改”状态
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="item">The entity item to set as modifed</param>
        void SetModified<T>(T item) where T : class;

        /// <summary>
        /// Apply current values in <paramref name="original"/>
        /// </summary>
        /// <typeparam name="T">The type of entity</typeparam>
        /// <param name="original">The original entity</param>
        /// <param name="current">The current entity</param>
        void ApplyCurrentValues<T>(T original, T current) where T : class;

    }
}
