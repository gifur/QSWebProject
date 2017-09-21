using QS.Core.IRepository;
using QS.Core.Module.SharedAggregate;
using QS.DAL;

namespace QS.Repository.Module
{

    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
