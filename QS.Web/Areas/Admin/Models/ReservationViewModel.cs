using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.DTO.ProfessionModule;

namespace QS.Web.Areas.Admin.Models
{
    public class ReservationViewModel
    {
        public int RId { get; set; }
        public string SubscriberName { get; set; }
        public string StuNumber { get; set; }
        public string Professional { get; set; }
        public string Phone { get; set; }
        public DateTime Dealtime { get; set; }
        public DateTime Createtime { get; set; }
        public DealState State { get; set; }
    }
}