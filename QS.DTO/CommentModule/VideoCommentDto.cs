using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.CommentModule
{
    public class VideoCommentDto : CommentDto
    {
        public Int64 VideoId { get; set; }

        public VideoCommentDto()
        {
            IsMember = 0;
        }
    }
}
