using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.Module;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface IRecentActivityService
    {
        void AddRecentActivity(RecentActivityDto activityDto);
        void DeleteRecentActivity(Int64 activityId);
        RecentActivityDto GetRecentActivityById(Int64 activityId);
        bool ChangeRecentActivityDescription(Int64 activityId, RecentActivityDto updatedRecentActivityDto);
        IEnumerable<RecentActivityDto> GetAllRecentActivitys();
        IEnumerable<RecentActivityDto> GetRecentActivityPaged(int pageIndex, int pageCount, out int count);
    }
}
