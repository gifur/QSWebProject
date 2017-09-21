using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Core.Module.CommentAggregate
{
    public class ArticleComment : Comment
    {
        public Int64 ArticleId { get; set; }
    }
}
