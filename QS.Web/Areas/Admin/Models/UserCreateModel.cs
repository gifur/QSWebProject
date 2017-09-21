using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Web.Areas.Admin.Controllers;
using QS.Web.Areas.Admin.Validations;

namespace QS.Web.Areas.Admin.Models
{
    public class UserCreateModel
    {
        public UserCreateModel()
        {
            StuNumber = string.Empty;
            RealName = string.Empty;
            Identification = string.Empty;
        }

        [MaxLength(32)]
        [Required(ErrorMessage=@"请输入学号")]
        [Display(Name = @"学号")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = @"请输入正确的学号")]
        [Remote("IsStuNumberAvailable", "UserManage", "Admin")]
        //[ScaffoldColumn(false)]
        [Editable(true)]
        public string StuNumber { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage=@"请输入真实姓名")]
        [Display(Name = @"真实姓名")]
        public string RealName { get; set; }

        [MaxLength(64)]
        [Required(ErrorMessage=@"请输入用户身份")]
        [Display(Name = @"身份")]
        public string Identification { get; set; }
    }
}