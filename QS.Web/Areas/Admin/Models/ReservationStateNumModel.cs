using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.DTO.ProfessionModule;

namespace QS.Web.Areas.Admin.Models
{
    /*  与Dto定义的关于状态的枚举变量同步  
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
     */
    public class ReservationStateNumModel
    {
        public ReservationStateNumModel(IEnumerable<ReservationDto> reservations)
        {
            Total = NumOfNew = NumOfFaraway = NumOfOvertime = 0;
            NumOfPending = NumOfTreated = NumOfUrgent = 0;
            var reservationDtos = reservations as IList<ReservationDto> ?? reservations.ToList();
            foreach (var item in reservationDtos)
            {
                if (item.State == DealState.New)
                {
                    ++NumOfNew;
                    ++NumOfPending;
                }
                if (item.State == DealState.Urgent)
                {
                    ++NumOfUrgent;
                    ++NumOfPending;
                }
                if (item.State == DealState.Pending)
                {
                    ++NumOfPending;
                }
                if (item.State == DealState.Overtime)
                {
                    ++NumOfOvertime;
                }
                if (item.State == DealState.Treated)
                {
                    ++NumOfTreated;
                }
                if (item.State == DealState.Faraway)
                {
                    ++NumOfTreated;
                    ++NumOfFaraway;
                }
                ++Total;
            }
        }
        public int Total { get; set; }
        public int NumOfNew { get; set; }
        public int NumOfUrgent { get; set; }
        public int NumOfPending { get; set; }
        public int NumOfTreated { get; set; }
        public int NumOfOvertime { get; set; }
        public int NumOfFaraway { get; set; }
    }
}