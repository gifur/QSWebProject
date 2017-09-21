using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QS.Web.Areas.Admin.Models
{
    public class FeedbackCreateModel
    {
        public FeedbackCreateModel()
        {
            DateTime dt = DateTime.Now;
            int year = dt.Year;//获取年份
            int month = dt.Month;//获取月份
            Title = String.Format("{0}年{1}月份心理反馈", year, month);

            StartTime = null;
            EndTime = null;
        }

        [StringLength(32, MinimumLength=6, ErrorMessage=@"标题长度至少有6个字符")]
        [Required(ErrorMessage=@"请输入标题")]
        public string Title { get; set; }

        [Required(ErrorMessage=@"请选择开始日期")]
        [DisplayFormat(NullDisplayText=@"点击选取日期")]
        [DataType(DataType.Date)]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage=@"请选择结束日期")]
        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }

    }
}