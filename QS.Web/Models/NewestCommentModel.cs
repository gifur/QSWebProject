using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    public class NewestCommentModel
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public Int64 Id { get; set; }
        public DateTime Time { get; set; }
    }
}