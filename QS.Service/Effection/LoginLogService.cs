using System;
using System.Collections.Generic;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.LogAggregate;
using QS.DTO.LogModule;


namespace QS.Service.Effection
{
    public class LoginLogService : ILoginLogService
    {
        private readonly ILoginLogRepository _loginLogRepository;
        public LoginLogService() { }

        public LoginLogService(ILoginLogRepository loginLogRepository)
        {
            _loginLogRepository = loginLogRepository;
        }

        public void DeleteLoginLog(Int64 loginLogId)
        {
            var temp = _loginLogRepository.Get(loginLogId);
            if (temp != null)
            {
                _loginLogRepository.Remove(temp);
                _loginLogRepository.UnitOfWork.Commit();
            }
        }

        public LoginLogDto GetLoginLogById(Int64 loginLogId)
        {
            var temp = _loginLogRepository.Get(loginLogId);
            return temp == null ? null : (QsMapper.CreateMap<LoginLog, LoginLogDto>(temp));
        }

        public IEnumerable<LoginLogDto> GetLoginLogPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageCount <= 0 || pageIndex <= 0)
            {
                count = 0;
                return null;
            }

            var loginLogEnumrable = _loginLogRepository.GetPaged(pageIndex, pageCount, out count, p => p.LoginTime, false);
            return QsMapper.CreateMapIEnume<LoginLog, LoginLogDto>(loginLogEnumrable);
        }

        public IEnumerable<LoginLogDto> GetAllLoginLogs()
        {
            var allLoginLog = _loginLogRepository.GetAllWithOrder(p => p.LoginTime);
            return QsMapper.CreateMapIEnume<LoginLog, LoginLogDto>(allLoginLog);
        }
    }
}
