using System;
using System.Collections.Generic;

namespace QS.Common
{
    /// <summary>
    /// The custom exception for validation errors
    /// </summary>
    public class ApplicationValidationErrorsException : Exception
    {
        #region Properties

        IEnumerable<string> _validationErrors;
        /// <summary>
        /// 获得或设置验证错误信息
        /// </summary>
        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 创建一个应用程序验证错误异常的新实例
        /// </summary>
        /// <param name="validationErrors">验证错误信息的集合</param>
        public ApplicationValidationErrorsException(IEnumerable<string> validationErrors)
            : base("无效类型, 期望的是一个已注册的映射的配置元素")
        {
            _validationErrors = validationErrors;
        }

        #endregion
    }
}
