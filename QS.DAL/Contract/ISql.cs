using System.Collections.Generic;

namespace QS.DAL.Contract
{
    /// <summary>
    /// Base contract for support 'dialect specific queries'.
    /// </summary>
    public interface ISql
    {
        /// <summary>
        /// Execute specific query with underliying persistence store
        /// </summary>
        /// <typeparam name="T">Entity type to map query results</typeparam>
        /// <param name="sqlQuery">
        /// Dialect Query 
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer > {0}
        /// </example>
        /// </param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// Enumerable results 
        /// </returns>
        IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters);
        IEnumerable<T> ExecuteQuery<T>(string sqlQuery);
        /// <summary>
        /// Execute arbitrary command into underliying persistence store
        /// </summary>
        /// <param name="sqlCommand">
        /// Command to execute
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer > {0}
        /// </example>
        ///</param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>受影响的行数</returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        int ExecuteCommand(string sqlCommand);
    }
}
