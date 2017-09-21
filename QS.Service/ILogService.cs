using System;
using System.Collections.Generic;
using QS.DTO.LogModule;

namespace QS.Service
{
    public interface ILogService
    {
        void DeleteLog(Int64 logId);
        LogDto GetLogById(Int64 logId);

        IEnumerable<LogDto> GetLogPaged(int pageIndex, int pageCount, out int count);
        IEnumerable<LogDto> GetAllLogs();
    }
}
