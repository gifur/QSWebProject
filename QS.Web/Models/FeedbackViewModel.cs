using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.DTO.FeedbackModule;

namespace QS.Web.Models
{
    public class FeedbackViewModel
    {
        public FeedbackViewModel()
        {
            Current = null;
            Record = null;
        }
        public FeedbackDto Current { get; set; }
        public FbDocumentDto Record { get; set; }
    }
}