using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.SharedModule
{
    public class PhotoDto
    {
        public PhotoDto()
        {
            Hits = 0;
            CommentNum = 0;
        }

        public Guid PhotoId { get; set; }
        public Guid AtlasId { get; set; }
        public string PhotoName { get; set; }
        public string PhotoTags { get; set; }
        public string ThumbPath { get; set; }
        public string PhotoPath { get; set; }

        [MaxLength(500, ErrorMessage = @"已超过字符限制")]
        public string Remark { get; set; }
        public int Hits { get; set; }
        public int CommentNum { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
