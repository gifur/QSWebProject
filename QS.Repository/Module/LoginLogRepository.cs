using QS.Core.IRepository;
using QS.Core.Module.LogAggregate;
using QS.DAL;

namespace QS.Repository.Module
{
    public class LoginLogRepository : Repository<LoginLog>, ILoginLogRepository
    {
        public LoginLogRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            
        }
    }
}
