using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.SharedAggregate
{
    public class Video : Entity, IValidatableObject
    {
        [Key]
        public Int64 VideoId { get; set; }
        public string VideoName { get; set; }
        public string ThumbPath { get; set; }
        public string VideoPath { get; set; }
        public string Remark { get; set; }
        public int Hits { get; set; }
        public int CommentNum { get; set; }
        public DateTime CreateTime { get; set; }
        public string Category { get; set; }
        public string ComesFrom { get; set; }
        public bool IsLocal { get; set; }
        public bool Recommend { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
