using QS.Core.IRepository;
using QS.Core.Module.SharedAggregate;
using QS.DAL;

namespace QS.Repository.Module
{

    public class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
