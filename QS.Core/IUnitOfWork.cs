using System;

namespace QS.Core
{
    /// <summary>
    /// Base interface to implement UnitOfWork Pattern.
    /// </summary>
    public interface IUnitOfWork
        : IDisposable
    {
        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,  
        /// then an exception is thrown
        ///</remarks>
        void Commit();

        /// <summary>
        /// Commit all changes made in  a container.
        /// </summary>
        ///<remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,
        /// then 'client changes' are refreshed - Client wins
        ///</remarks>
        void CommitAndRefreshChanges();


        /// <summary>
        /// Rollback tracked changes. See references of UnitOfWork pattern
        /// </summary>
        void RollbackChanges();

    }
}
