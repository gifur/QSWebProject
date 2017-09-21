using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.Module;

namespace QS.DTO.FeedbackModule
{
    public class FbDocumentDto
    {
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public int FeedbackId { get; set; }
        public int UploaderId { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploaderName { get; set; }
    }
}
