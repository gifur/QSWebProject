using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.FeedbackAggregate
{
    public partial class Feedback : Entity
    {
        #region Constructor
        public Feedback()
        {
            FbDocuments = new HashSet<FbDocument>();
        }
        #endregion

        #region Property
        [Key]
        public int FeedbackId { get; set; }
        public string FeedbackName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual ICollection<FbDocument> FbDocuments { get; set; }

        #endregion
    }
}
