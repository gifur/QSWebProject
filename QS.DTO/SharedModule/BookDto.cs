using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.SharedModule
{
    public class BookDto
    {
        public Int64 BookId { get; set; }

        [Display(Name=@"书籍名称")]
        [Required(ErrorMessage=@"请填写书籍的名称")]
        [StringLength(50, ErrorMessage=@"书籍名称不超过50个字")]
        public string BookName { get; set; }

        [Required(ErrorMessage = @"简要描述是必须填写的")]
        [StringLength(80, ErrorMessage = @"请注意，描述不超过80个字符")]
        public string Remark { get; set; }
        public string Category { get; set; }
        public string ThumbPath { get; set; }
        public string CoverPath { get; set; }
        [Required(ErrorMessage=@"请填写作者名字或填写'佚名'")]
        [StringLength(20, ErrorMessage = @"作者姓名不超过20个字")]
        public string Author { get; set; }
        [Required(ErrorMessage = @"请填写出版社名称")]
        [StringLength(50, ErrorMessage = @"出版社名称不超过20个字")]
        public string Press { get; set; }
        public string PublishedTime { get; set; }
        public Int32? PageNum { get; set; }
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

        public BookDto()
        {
            Hits = 0;
            CommentNum = 0;
            Grade = 0;
            Already = Wish = Reading = 0;
        }
    }
}
