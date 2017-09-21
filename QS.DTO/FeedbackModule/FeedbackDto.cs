using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QS.DTO.FeedbackModule
{
    public enum EnumFbStatus
    {
        /// <summary>
        /// 还未开始
        /// </summary>
        Await = -1,
        /// <summary>
        /// 进行中
        /// </summary>
        Underway = 1,
        /// <summary>
        /// 已结束
        /// </summary>
        Closed = 0,
        /// <summary>
        /// 已被终止
        /// </summary>
        Aborted = -2
    }

    public class FeedbackDto
    {
        public FeedbackDto()
        {
            CreateTime = DateTime.Now;
            Status = EnumFbStatus.Await;
            //FbDocumentDtos = new List<FbDocumentDto>();
        }
        public int FeedbackId { get; set; }
        public string FeedbackName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndTime { get; set; }
        public  EnumFbStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        //public List<FbDocumentDto> FbDocumentDtos { get; set; }

        /// <summary>
        /// 根据时间判断反馈的进行状态
        /// </summary>
        /// <returns>返回True表示状态有所改变，返回False则无</returns>
        public bool JudgeStatus()
        {
            var currentTime = DateTime.Now;
            if (StartTime > currentTime)
            {
                if (Status == EnumFbStatus.Await)
                    return false;
            }
            else if (StartTime <= currentTime && currentTime < EndTime.AddDays(1))
            {
                if (Status == EnumFbStatus.Underway)
                    return false;

                Status = EnumFbStatus.Underway;
                return true;
            }

            if (currentTime < EndTime.AddDays(1)) return false;
            if (Status == EnumFbStatus.Closed)
                return false;

            Status = EnumFbStatus.Closed;
            return true;
        }
    }
}
