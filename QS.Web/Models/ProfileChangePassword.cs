using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    public class ProfileChangePassword
    {
        public ProfileChangePassword()
        {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
        }

        [StringLength(32)]
        [Required(ErrorMessage = @"请输入原始密码")]
        [Display(Name = @"原密码")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [StringLength(32, ErrorMessage = @"密码长度至少为6", MinimumLength = 6)]
        [Required(ErrorMessage=@"请输入新密码")]
        [Display(Name = @"新密码")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage=@"两次输入的密码不一致")]
        [Display(Name = @"确认密码")]
        [Required(ErrorMessage = @"请再次输入新密码")]
        public string ConfirmNewPassword { get; set; }

        public void Trim()
        {
            if (!string.IsNullOrEmpty(CurrentPassword)) CurrentPassword = CurrentPassword.Trim();
            if (!string.IsNullOrEmpty(NewPassword)) NewPassword = NewPassword.Trim();
            if (!string.IsNullOrEmpty(ConfirmNewPassword)) ConfirmNewPassword = ConfirmNewPassword.Trim();
        }
    }
}