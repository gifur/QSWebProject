using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.SharedAggregate
{
    public partial class Article : Entity, IValidatableObject
    {
        [Key]
        public Int64 ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string Category { get; set; }
        public bool IsTop { get; set; }
        public string ArticleContent { get; set; }
        public int ViewTimes { get; set; }
        public int CommentNum { get; set; }
        public string ArticleTags { get; set; }
        public DateTime CreateTime { get; set; }
        public string ThumbPath { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
