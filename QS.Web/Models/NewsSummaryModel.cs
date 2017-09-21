using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    public class NewsSummaryModel
    {
        public Int64 NewsId { get; set; }

        public string NewsTitle { get; set; }

        public string Category { get; set; }

        public string ViewImage { get; set; }

        //0（false)表示普通， 1（true)表示置顶
        public bool IsTop { get; set; }

        public string NewsContent { get; set; }

        public int ViewTimes { get; set; }

        public int CommentNum { get; set; }

        public string NewsTags { get; set; }

        public DateTime CreateTime { get; set; }
    }
}