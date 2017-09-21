using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.Common;
using QS.Core.Module.FeedbackAggregate;
using QS.Core.Resource;

namespace QS.Core.Module
{

    public class MyMessage : Entity, IValidatableObject
    {
        [Key]
        public Int64 MyId { get; set; }
        public Int64 MId { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public DateTime? RecentTime { get; set; }
        public virtual User User { get; set; }
        public virtual Message Message{ get; set; }

        #region IValidatableObject Members

        /// <summary>
        /// 验证必要信息
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }

        #endregion
    }
}
