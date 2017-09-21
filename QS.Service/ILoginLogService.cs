using System;
using System.Collections.Generic;
using QS.DTO.LogModule;

namespace QS.Service
{
    public interface ILoginLogService
    {
        void DeleteLoginLog(Int64 loginLogId);
        LoginLogDto GetLoginLogById(Int64 loginLogId);

        IEnumerable<LoginLogDto> GetLoginLogPaged(int pageIndex, int pageCount, out int count);
        IEnumerable<LoginLogDto> GetAllLoginLogs();
    }
}
