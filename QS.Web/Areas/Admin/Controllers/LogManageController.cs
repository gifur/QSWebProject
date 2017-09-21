using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class LogManageController : Controller
    {
        private readonly ILoginLogService _loginLogService;
        public LogManageController() { }

        public LogManageController(ILoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserLogin(int id = 1)
        {
            int count;
            const int pageSize = 12;
            var result = _loginLogService.GetAllLoginLogs();
            var model = result.ToPagedList(id, pageSize);
            return View(model);
        }
    }
}
