using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.CommentModule
{
    public class ArticleCommentDto : CommentDto
    {
        public Int64 ArticleId { get; set; }

        public ArticleCommentDto()
        {
            IsMember = 0;
        }
    }
}
