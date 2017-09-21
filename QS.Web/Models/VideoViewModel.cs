using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.DTO.SharedModule;

namespace QS.Web.Models
{
    /// <summary>
    /// 在视频主页面显示的可视类
    /// </summary>
    public class VideoViewModel
    {
        public int Count { get; set; }
        public string Category { get; set; }
        public string DivId { get; set; }
        public IEnumerable<VideoDto> Contents { get; set; }

        public VideoViewModel()
        {
            Contents = new List<VideoDto>();
            Count = 0;
        }

    }
}