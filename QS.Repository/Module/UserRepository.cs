using QS.Core.IRepository;
using QS.Core.Module;
using QS.DAL;

namespace QS.Repository.Module
{
    /// <summary>
    /// 在建立XXRepository实例时，需要将实现了IUnitWork接口的数据上下文对象传进来，然后统一进行操作。
    /// 在BLL层调用DAL层时，可以将多个DAL层的Repository对象进行组件调用，
    /// 只要保证IUnitwork上下文对象是唯一的
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
