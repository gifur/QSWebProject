using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using QS.DTO.Module;

namespace QS.Web.Models
{
    public class AcntWithoutPsw
    {
        public int UserId { get; set; }

        [RegularExpression(@"(\S)+", ErrorMessage = @"不允许输入空白字符")]
        [Required(ErrorMessage=@"用户名不能为空")]
        [Remote("IsUserNameAvailable", "Account")]
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string StuNumber { get; set; }
        public string Identification { get; set; }

        [Range(0, 2, ErrorMessage = @"请选择性别")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = @"请输入联系电话")]
        public string Phone { get; set; }

        [Email(ErrorMessage = @"输入内容不符合邮箱格式")]
        [Required(ErrorMessage = @"请输入邮箱地址")]
        public string Email { get; set; }
        public string About { get; set; }
        [MaxLength(64, ErrorMessage=@"已经超过长度限制")]
        public string PersonalPage { get; set; }
    }
}