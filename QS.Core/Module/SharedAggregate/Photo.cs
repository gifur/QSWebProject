using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.SharedAggregate
{
    public partial class Photo : Entity, IValidatableObject
    {
        [Key]
        public Guid PhotoId { get; set; }
        public Guid AtlasId { get; set; }
        public string PhotoName { get; set; }
        public string PhotoTags { get; set; }
        public string ThumbPath { get; set; }
        public string PhotoPath { get; set; }
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
