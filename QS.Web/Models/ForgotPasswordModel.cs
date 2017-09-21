using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace QS.Web.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = @"真实姓名不能为空")]
        public string RealName { get; set; }

        [Email(ErrorMessage = @"输入内容不符合邮箱格式")]
        [Required(ErrorMessage = @"请输入邮箱地址")]
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
    }
}