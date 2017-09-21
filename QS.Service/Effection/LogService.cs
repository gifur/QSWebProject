using System;
using System.Collections.Generic;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.LogAggregate;
using QS.DTO.LogModule;


namespace QS.Service.Effection
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService() { }

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void DeleteLog(Int64 logId)
        {
            var temp = _logRepository.Get(logId);
            if (temp != null)
            {
                _logRepository.Remove(temp);
                _logRepository.UnitOfWork.Commit();
            }
        }

        public LogDto GetLogById(Int64 logId)
        {
            var temp = _logRepository.Get(logId);
            return temp == null ? null : (QsMapper.CreateMap<Log, LogDto>(temp));
        }

        public IEnumerable<LogDto> GetLogPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageCount <= 0 || pageIndex <= 0)
            {
                count = 0;
                return null;
            }

            var logEnumrable = _logRepository.GetPaged(pageIndex, pageCount, out count, p => p.Date, false);
            return QsMapper.CreateMapIEnume<Log, LogDto>(logEnumrable);
        }

        public IEnumerable<LogDto> GetAllLogs()
        {
            var allLog = _logRepository.GetAllWithOrder(p => p.Date);
            return QsMapper.CreateMapIEnume<Log, LogDto>(allLog);
        }
    }
}
