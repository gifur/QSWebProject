using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnnotationsExtensions;

namespace QS.DTO.CommentModule
{
    public class SuggestionDto
    {
        public Int64 Id { get; set; }
        [MaxLength(15, ErrorMessage=@"不好意思，长度超过限制")]
        [Required(ErrorMessage = @"人的名树的影，您怎么称呼？")]
        public string NickName { get; set; }
        [Email(ErrorMessage = @"输入内容不符合邮箱格式")]
        [Required(ErrorMessage = @"请输入邮箱地址")]
        public string Email { get; set; }
        [Required(ErrorMessage = @"请填写评价或建议的内容")]
        [MaxLength(1000, ErrorMessage=@"已超过评议字数的限制")]
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
