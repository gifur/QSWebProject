using QS.Core.IRepository;
using QS.Core.Module;
using QS.DAL;

namespace QS.Repository.Module
{

    public class RecentActivityRepository : Repository<RecentActivity>, IRecentActivityRepository
    {
        public RecentActivityRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
