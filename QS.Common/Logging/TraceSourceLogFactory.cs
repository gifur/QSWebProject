using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Common.Logging
{
    /// <summary>
    /// A Trace Source base, log factory
    /// </summary>
    public class TraceSourceLogFactory : ILoggerFactory
    {
        /// <summary>
        /// 创建跟踪源日志
        /// </summary>
        /// <returns>基于跟踪源架构的新日志接口</returns>
        public ILogger Create()
        {
            return new TraceSourceLog();
        }
    }
}
