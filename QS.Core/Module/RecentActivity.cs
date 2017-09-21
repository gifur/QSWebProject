using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module
{
    public class RecentActivity : Entity, IValidatableObject
    {
        [Key]
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Status { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
