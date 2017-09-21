using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.LogModule
{
    public class LogDto
    {
        public Int64 Id { get; set; }
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public DateTime Message { get; set; }
        public string Exception { get; set; }
    }
}
