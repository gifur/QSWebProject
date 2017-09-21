using QS.Core.IRepository;
using QS.Core.Module;
using QS.DAL;

namespace QS.Repository.Module
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
