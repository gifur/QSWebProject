using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.SharedModule
{
    public class ArticleDto
    {
        public Int64 ArticleId { get; set; }

        [Required(ErrorMessage=@"请填写文章标题")]
        [StringLength(100, ErrorMessage = @"{0} 必须至少包含 {2} 个字符。", MinimumLength = 4)]
        public string ArticleTitle { get; set; }
        [Required(ErrorMessage = @"请选择文章分类")]
        public string Category { get; set; }

        //0（false)表示普通， 1（true)表示置顶
        public bool IsTop { get; set; }

        [Required(ErrorMessage = @"请输入文章内容")]

        public string ArticleContent { get; set; }
        public int ViewTimes { get; set; }
        public int CommentNum { get; set; }
        //[Required(ErrorMessage = @"请选择所属标签")]
        public string ArticleTags { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateTime { get; set; }
        public string ThumbPath { get; set; }
        
    }
}
