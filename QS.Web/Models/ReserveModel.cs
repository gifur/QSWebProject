using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using QS.DTO.ProfessionModule;

namespace QS.Web.Models
{
    public class ReserveModel
    {
        public ReserveModel()
        {
            Gender = GenderType.Security;
            Dealtime = null;
            Age = null;
        }
        [MaxLength(32, ErrorMessage = @"已经超过长度限制")]
        [Required(ErrorMessage = @"请输入姓名")]
        public string SubscriberName { get; set; }

        [MaxLength(12, ErrorMessage = @"学号长度不符合要求")]
        [RegularExpression(@"^\d{12}$", ErrorMessage=@"学号不符合要求")]
        [Required(ErrorMessage = @"请输入学号")]
        public string StuNumber { get; set; }

        //[Range(0, 2, ErrorMessage = @"请选择性别")]
        public GenderType Gender { get; set; }
        public int? Age { get; set; }
        [MaxLength(64, ErrorMessage = @"年级专业班级已超过长度限制")]
        [Required(ErrorMessage = @"请输入所在年级专业班级")]
        public string Professional { get; set; }

        [Required(ErrorMessage = @"请输入联系电话")]
        public string Phone { get; set; }
        [Email(ErrorMessage = @"输入内容不符合邮箱格式")]
        public string Email { get; set; }
        [MaxLength(128, ErrorMessage = @"过往情况已超过长度限制")]
        public string Past { get; set; }
        [MaxLength(128, ErrorMessage = @"咨询经历已超过长度限制")]
        public string Experience { get; set; }

        [Required(ErrorMessage = @"请选择你想要预约的时间")]
        [DataType(DataType.Date)]
        public DateTime? Dealtime { get; set; }
        [MaxLength(2000, ErrorMessage = @"情况描述已超过长度限制")]
        public string Situation { get; set; }
    }
}