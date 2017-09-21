using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.DTO.FeedbackModule;

namespace QS.DTO.Module
{

    public class MessageDto
    {
        public Int64 MId { get; set; }
        [Required(ErrorMessage = @"请填写通知标题")]
        [StringLength(100, ErrorMessage = @"{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        public string Title { get; set; }
        [Required(ErrorMessage = @"请输入通知内容")]
        public string Context { get; set; }
        public string Appendix { get; set; }
        public string Type { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public virtual ICollection<MyMessageDto> MyMessageDtos { get; set; }
    }
}
