using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.Common;
using QS.Core.Module.FeedbackAggregate;
using QS.Core.Resource;

namespace QS.Core.Module
{

    public class Tag : Entity, IValidatableObject
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string TagEnglish { get; set; }
        public string TagDescription { get; set; }
        public int Belong { get; set; }
        public Int64 TagSum { get; set; }
        public DateTime CreateTime { get; set; }


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
