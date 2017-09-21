using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Common.Validator
{
    /// <summary>
    /// 实体验证工厂的数据注入
    /// </summary>
    public class DataAnnotationsEntityValidatorFactory
        : IEntityValidatorFactory
    {
        /// <summary>
        /// 创建一个验证类实体
        /// </summary>
        /// <returns></returns>
        public IEntityValidator Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
