using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.SharedAggregate
{
    public partial class Atlas : Entity, IValidatableObject
    {
        [Key]
        public Guid AtlasId { get; set; }
        public string AtlasName { get; set; }
        public string ThumbPath { get; set; }
        public string AtlasPath { get; set; }
        public string Remark { get; set; }
        public int Hits { get; set; }
        public int CommentNum { get; set; }
        public DateTime CreateTime { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
