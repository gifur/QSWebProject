using System;
using System.ComponentModel.DataAnnotations;
using QS.Common;

namespace QS.Core.Module.FeedbackAggregate
{
    public partial class FbDocument : Entity
    {
        #region Property

        [Key]
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public int FeedbackId { get; set; }
        public int UploaderId { get; set; }
        public DateTime UploadDate { get; set; }

        public virtual Feedback Feedback { get; set; }
        public virtual User Uploader { get; set; }

        #endregion Property

        #region Method

        public void AssociateFeedbackForThisDocument(Feedback feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException();
            }

            FeedbackId = feedback.FeedbackId;
            Feedback = feedback;
        }

        public void SetTheFeedbackReference(int feedbackId)
        {
            if (feedbackId > 0)
            {
                FeedbackId = feedbackId;
                Feedback = null;
            }
        }

        public void AssociateUploaderForThisDocument(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException();
            }

            this.UploaderId = user.UserId;
            this.Uploader = user;
        }

        public void SetTheUploaderReference(int userId)
        {
            if (userId > 0)
            {
                UploaderId = userId;
                Uploader = null;
            }
        }

        #endregion Method
    }
}
