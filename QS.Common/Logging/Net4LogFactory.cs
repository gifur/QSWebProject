namespace QS.Common.Logging
{
    /// <summary>
    /// Log4Net的工厂类
    /// </summary>
    public class Net4LogFactory : ILoggerFactory
    {
        /// <summary>
        /// 创建跟踪源日志
        /// </summary>
        /// <returns>基于跟踪源架构的新日志接口</returns>
        public ILogger Create()
        {
            return new Net4Log();
        }
    }
}
