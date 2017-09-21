using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.DTO.FeedbackModule;

namespace QS.Service
{
    public interface IFeedbackService
    {
        FeedbackDto GetFeedbackById(int id);
        void AddFeedback(FeedbackDto feedbackDto);
        void DeleteFeedback(int feedbackId);

        OutMsg UpdateFeedbackDetail(int id, FeedbackDto feedbackDto);

        List<FeedbackDto> FindFeedbacks(int pageIndex, int pageCount);

        List<FeedbackDto> GetAllFeedback();

        FeedbackDto GetActiveItem();
    }
}
