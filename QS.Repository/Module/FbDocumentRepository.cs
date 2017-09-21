using QS.Core.IRepository;
using QS.Core.Module.FeedbackAggregate;
using QS.DAL;

namespace QS.Repository.Module
{
    public class FbDocumentRepository : Repository<FbDocument>, IFbDocumentRepository
    {
        public FbDocumentRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
