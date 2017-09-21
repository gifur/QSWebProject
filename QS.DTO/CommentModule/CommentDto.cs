using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnnotationsExtensions;
using QS.DTO.Module;

namespace QS.DTO.CommentModule
{
    public class CommentDto
    {
        public Int64 CommentId { get; set; }
        public Int64 UpId { get; set; }
        public string NickName { get; set; }
        [Email(ErrorMessage = @"输入内容不符合邮箱格式")]
        [Required(ErrorMessage = @"请输入邮箱地址")]
        public string Email { get; set; }
        [Required(ErrorMessage = @"请写点东西吧")]
        public string Content { get; set; }
        public int IsMember { get; set; }
        public DateTime CreateTime { get; set; }
        public string PhotoUrl { get; set; }
        public string Identification { get; set; }
        public string UniqueKey { get; set; }
    }
}
