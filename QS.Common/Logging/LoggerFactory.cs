using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Common.Logging
{
    /// <summary>
    /// 日志工厂
    /// </summary>
    public static class LoggerFactory
    {
        #region Members

        static ILoggerFactory _currentLogFactory = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="logFactory">使用的日志工厂</param>
        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        /// <summary>
        /// 创建一个新的日志
        /// </summary>
        /// <returns>返回一个日志接口类型</returns>
        public static ILogger CreateLog()
        {
            return (_currentLogFactory != null) ? _currentLogFactory.Create() : null;
        }

        #endregion
    }
}
