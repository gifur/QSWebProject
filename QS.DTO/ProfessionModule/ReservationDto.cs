using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS.DTO.ProfessionModule
{
    public enum GenderType
    {
        /// <summary>
        /// 男
        /// </summary>
        Male = 0,

        /// <summary>
        /// 女
        /// </summary>
        Female = 1,

        /// <summary>
        /// 保密
        /// </summary>
        Security = 2
    }

    /// <summary>
    /// 设想是在用户查看Index页面时运行一次写好的存储过程，使得能及时改变状态，因为仅限制于日期
    /// 所以打消该想法，改为数据库通过Job设计一天更新一次状态
    /// </summary>
    public enum DealState
    {
        /// <summary>
        /// 新  不超过创建时间一天
        /// </summary>
        New = 0,
        /// <summary>
        /// 紧急 处于未处理状态的前提下：超过创建时间两天，或离预约时间仅剩下一天
        /// </summary>
        Urgent = 1,
        /// <summary>
        /// 未处理 处于未处理状态下：排除上面说的两种情况
        /// </summary>
        Pending = 2,
        /// <summary>
        /// 已处理
        /// </summary>
        Treated = 3,
        /// <summary>
        /// 已超时 处于未处理状态下，超过预约的时间
        /// </summary>
        Overtime = 4,
        /// <summary>
        /// 久远的 处于已处理的状态下，超过创建时间一个月（30天）的时间
        /// </summary>
        Faraway = 5
    }
    public class ReservationDto
    {
        public int RId { get; set; }
        public string SubscriberName { get; set; }
        public string StuNumber { get; set; }
        public GenderType Gender { get; set; }
        public int Age { get; set; }
        public string Professional { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Past { get; set; }
        public string Experience { get; set; }
        public DateTime Dealtime { get; set; }
        public string Situation { get; set; }
        public DateTime Createtime { get; set; }
        public DealState State { get; set; }
    }
}
