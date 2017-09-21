
namespace QS.Common.Validator
{
    /// <summary>
    /// Entity Validator Factory
    /// </summary>
    public static class EntityValidatorFactory
    {
        #region Members

        static IEntityValidatorFactory _factory = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// 设置使用的日志工厂
        /// </summary>
        /// <param name="factory">使用的日志工厂</param>
        public static void SetCurrent(IEntityValidatorFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// 创建 
        /// </summary>
        /// <returns>创建日志接口</returns>
        public static IEntityValidator CreateValidator()
        {
            return (_factory != null) ? _factory.Create() : null;
        }

        #endregion
    }
}
