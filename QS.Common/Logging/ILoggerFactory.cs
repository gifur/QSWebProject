using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Common.Logging
{
    /// <summary>
    /// 抽象日志工厂的基础构建
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// 创建一个日志对象
        /// </summary>
        /// <returns>The ILog created</returns>
        ILogger Create();
    }
}
