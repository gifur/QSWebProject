using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.SharedModule
{
    public class NewsDto
    {
        public Int64 NewsId { get; set; }

        [Required(ErrorMessage=@"请填写新闻标题")]
        [StringLength(100, ErrorMessage = @"{0} 必须至少包含 {2} 个字符。", MinimumLength = 4)]
        public string NewsTitle { get; set; }
        [Required(ErrorMessage = @"请选择新闻分类")]
        public string Category { get; set; }

        //0（false)表示普通， 1（true)表示置顶
        public bool IsTop { get; set; }

        [Required(ErrorMessage = @"请输入新闻内容")]

        public string NewsContent { get; set; }
        public int ViewTimes { get; set; }
        public int CommentNum { get; set; }
        //[Required(ErrorMessage = @"请选择所属标签")]
        public string NewsTags { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateTime { get; set; }

        public string ThumbPath { get; set; }
    }
}
