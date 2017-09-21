using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.DTO.FeedbackModule;

namespace QS.DTO.Module
{

    public class MyMessageDto
    {
        public Int64 MyId { get; set; }
        public Int64 MId { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public DateTime? RecentTime { get; set; }
        public virtual UserDto UserDto { get; set; }
        public virtual MessageDto MessageDto { get; set; }
    }
}
