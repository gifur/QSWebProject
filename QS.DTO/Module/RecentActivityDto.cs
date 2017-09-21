using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.Module
{
    public class RecentActivityDto
    {
        public Int64 Id { get; set; }
        [MaxLength(50, ErrorMessage = @"不好意思，长度超过限制")]
        [Required(ErrorMessage = @"请填写活动标题")]
        public string Title { get; set; }
        [Required(ErrorMessage = @"请填写必要的内容")]
        [MaxLength(1000, ErrorMessage = @"已超过字数限制")]
        public string Content { get; set; }
        [DataType(DataType.DateTime, ErrorMessage=@"时间格式不对")]
        [Required(ErrorMessage = @"请选择活动开始的精确时间")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = @"请填写活动的地址")]
        [MaxLength(200, ErrorMessage = @"已超过字数限制")]
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Status { get; set; }

        public RecentActivityDto()
        {
            Status = true;
        }
    }
}
