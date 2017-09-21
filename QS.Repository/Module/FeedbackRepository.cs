using QS.Core.IRepository;
using QS.Core.Module.FeedbackAggregate;
using QS.DAL;

namespace QS.Repository.Module
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
