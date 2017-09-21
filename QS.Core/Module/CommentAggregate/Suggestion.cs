using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
namespace QS.Core.Module.CommentAggregate
{
    public class Suggestion : Entity, IValidatableObject
    {
        [Key]
        public Int64 Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
