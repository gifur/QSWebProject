using System;
using System.Collections.Generic;
using System.Linq;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module;
using QS.DTO.Module;

namespace QS.Service.Effection
{
    public class RecentActivityService : IRecentActivityService
    {
        private readonly IRecentActivityRepository _activityRepository;

        public RecentActivityService() { }

        public RecentActivityService(IRecentActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }
        public void AddRecentActivity(RecentActivityDto activityDto)
        {
            activityDto.CreateTime = DateTime.Now;
            _activityRepository.Add(QsMapper.CreateMap<RecentActivityDto, RecentActivity>(activityDto));
            _activityRepository.UnitOfWork.Commit();
        }

        public void DeleteRecentActivity(Int64 activityId)
        {
            var temp = _activityRepository.Get(activityId);
            if (temp == null) return;
            _activityRepository.Remove(temp);
            _activityRepository.UnitOfWork.Commit();
        }

        public RecentActivityDto GetRecentActivityById(Int64 activityId)
        {
            var temp = _activityRepository.Get(activityId);
            return temp == null ? new RecentActivityDto() : (QsMapper.CreateMap<RecentActivity, RecentActivityDto>(temp));
        }

        public bool ChangeRecentActivityDescription(Int64 activityId, RecentActivityDto updatedRecentActivityDto)
        {
   
            var original = _activityRepository.Get(activityId);
            var recent = QsMapper.CreateMap<RecentActivityDto, RecentActivity>(updatedRecentActivityDto);
            if (original != null && recent != null)
            {
                _activityRepository.Merge(original, recent);
                _activityRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<RecentActivityDto> GetAllRecentActivitys()
        {
            var allRecentActivity = _activityRepository.GetAll().OrderByDescending(n => n.StartTime).AsEnumerable();
            //var allRecentActivity = _activityRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<RecentActivity, RecentActivityDto>(allRecentActivity);
        }

        public IEnumerable<RecentActivityDto> GetRecentActivityPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            var activityEnumrable = _activityRepository.GetPaged(pageIndex, pageCount, out count, n => n.StartTime, false); 
            //var activityEnumrable = _activityRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<RecentActivity, RecentActivityDto>(activityEnumrable);
        }

    }
}
