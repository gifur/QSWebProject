using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.Common;

namespace QS.Core.Module.LogAggregate
{
    public sealed partial class LoginLog : Entity, IValidatableObject
    {
        public Int64 LoginLogId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }
        public string ComputerName { get; set; }
        public DateTime LoginTime { get; set; }
        public string Platform { get; set; }
        public string UserAgent { get; set; }
        public string Type { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
