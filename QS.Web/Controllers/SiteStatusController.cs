using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace QS.Web.Controllers
{
    public class SiteStatusController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(HttpStatusCode? code)
        {
            throw new HttpException(404, @"Show页面不存在");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult InternalError()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }
    }
}
