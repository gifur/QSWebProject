using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Core.Module.CommentAggregate
{
    public class BookComment : Comment
    {
        public Int64 BookId { get; set; }
        public decimal Score { get; set; }
        public int ReadStatus { get; set; }
    }
}
