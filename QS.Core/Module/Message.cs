using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.Common;
using QS.Core.Module.FeedbackAggregate;
using QS.Core.Resource;

namespace QS.Core.Module
{

    public class Message : Entity, IValidatableObject
    {
        [Key]
        public Int64 MId { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Appendix { get; set; }
        public string Type { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public virtual ICollection<MyMessage> MyMessages { get; set; }

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
