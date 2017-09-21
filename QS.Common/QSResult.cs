using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.Common
{
    public class QsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public QsResult()
        {
            Success = true;
            Message = @"暂无对结果的相关描述...";
        }
    }
}
