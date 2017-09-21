using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.SharedModule
{
    public class AtlasDto
    {
        public AtlasDto()
        {
            Hits = 0;
            CommentNum = 0;
        }

        public Guid AtlasId { get; set; }
        public string AtlasName { get; set; }
        public string ThumbPath { get; set; }
        public string AtlasPath { get; set; }
        public string Remark { get; set; }
        public int Hits { get; set; }
        public int CommentNum { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
