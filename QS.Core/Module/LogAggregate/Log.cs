using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.Common;

namespace QS.Core.Module.LogAggregate
{
    public sealed partial class Log : Entity, IValidatableObject
    {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public DateTime Message { get; set; }
        public string Exception { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
