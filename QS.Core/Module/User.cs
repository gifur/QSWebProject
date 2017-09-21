using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.Common;
using QS.Core.Module.FeedbackAggregate;
using QS.Core.Resource;

namespace QS.Core.Module
{

    public sealed partial class User : Entity, IValidatableObject
    {
        public User()
        {
            FbDocuments = new HashSet<FbDocument>();
        }

        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string StuNumber { get; set; }
        public string Identification { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string About { get; set; }
        public string PersonalPage { get; set; }
        public int State { get; set; }
        public string Roles { get; set; }

        public ICollection<FbDocument> FbDocuments { get; set; }

        #region IValidatableObject Members

        /// <summary>
        /// 验证必要信息
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }

        #endregion
    }
}
