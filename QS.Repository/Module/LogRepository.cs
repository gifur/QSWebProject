using QS.Core.IRepository;
using QS.Core.Module.LogAggregate;
using QS.DAL;

namespace QS.Repository.Module
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            
        }
    }
}
