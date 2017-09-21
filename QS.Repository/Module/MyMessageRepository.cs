using QS.Core.IRepository;
using QS.Core.Module;
using QS.DAL;

namespace QS.Repository.Module
{
    public class MyMessageRepository : Repository<MyMessage>, IMyMessageRepository
    {
        public MyMessageRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
