using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = @"请输入用户名或学号")]
        public string NameOrNumber { get; set; }
        [StringLength(32, MinimumLength = 6, ErrorMessage=@"至少有6位数")]
        [Required(ErrorMessage = @"请输入密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = @"请输入验证码")]
        public string ValidateCode { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}