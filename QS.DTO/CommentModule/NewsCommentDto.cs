using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.CommentModule
{
    public class NewsCommentDto : CommentDto
    {
        public Int64 NewsId { get; set; }

        public NewsCommentDto()
        {
            IsMember = 0;
        }
    }
}
