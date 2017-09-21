using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.SharedModule
{
    public class VideoDto
    {
        public Int64 VideoId { get; set; }
        [Required(ErrorMessage=@"请输入视频名字")]
        [MaxLength(50, ErrorMessage=@"最多输入50个字符")]
        public string VideoName { get; set; }
        public string ThumbPath { get; set; }
        [Required(ErrorMessage=@"请填入视频链接或本地地址")]
        public string VideoPath { get; set; }
        [MaxLength(500, ErrorMessage=@"最多输入500个字符进行描述")]
        public string Remark { get; set; }
        public int Hits { get; set; }
        public int CommentNum { get; set; }
        public DateTime CreateTime { get; set; }
        public string Category { get; set; }
        [MaxLength(100, ErrorMessage = @"最多输入100个字符")]
        [Required(ErrorMessage = @"请输入视频名字")]
        public string ComesFrom { get; set; }
        public bool IsLocal { get; set; }
        public bool Recommend { get; set; }
    }
}
