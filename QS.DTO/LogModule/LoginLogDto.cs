using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.LogModule
{
    public class LoginLogDto
    {
        public Int64 LoginLogId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }
        public string ComputerName { get; set; }
        public DateTime LoginTime { get; set; }
        public string Platform { get; set; }
        public string UserAgent { get; set; }
        public string Type { get; set; }
    }
}
