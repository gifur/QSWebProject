using QS.Core.IRepository;
using QS.Core.Module;
using QS.DAL;

namespace QS.Repository.Module
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
