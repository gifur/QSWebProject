using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.CommentModule
{
    public enum ReadStatus
    {
        /// <summary>
        /// 读过
        /// </summary>
        Already = 3,
        /// <summary>
        /// 想读
        /// </summary>
        Wish = 1,
        /// <summary>
        /// 在读
        /// </summary>
        Reading = 2
    }
    public class BookCommentDto : CommentDto
    {
        public Int64 BookId { get; set; }
        public decimal Score { get; set; }
        [Required(ErrorMessage=@"请选择您对此书目前的状态吧")]
        [Range(1, 3, ErrorMessage=@"为更好地体现您的意向，请选择您对此书目前的状态吧")]
        public ReadStatus ReadStatus { get; set; }

        public BookCommentDto()
        {
            IsMember = 0;
            Score = 0;
        }
    }
}
