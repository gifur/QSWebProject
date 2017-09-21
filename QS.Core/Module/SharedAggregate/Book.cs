using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.SharedAggregate
{
    public class Book : Entity, IValidatableObject
    {
        [Key]
        public Int64 BookId { get; set; }
        public string BookName { get; set; }
        public string Remark { get; set; }
        public string Category { get; set; }
        public string ThumbPath { get; set; }
        public string CoverPath { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public string PublishedTime { get; set; }
        public int PageNum { get; set; }
        public decimal Grade { get; set; }
        public int EvaluateTimes { get; set; }
        public int Hits { get; set; }
        public bool HasResource { get; set; }
        public string ResourcePath { get; set; }
        public string BookDescribing { get; set; }
        public string AuthorDepict { get; set; }
        public int CommentNum { get; set; }
        public DateTime CreateTime { get; set; }
        public int Already { get; set; }
        public int Wish { get; set; }
        public int Reading { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
