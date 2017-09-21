using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.Module;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class ActivityManageController : BaseController
    {
        private readonly IRecentActivityService _recentService;
        public ActivityManageController() { }

        public ActivityManageController(IRecentActivityService recentService)
        {
            _recentService = recentService;
        }

        public ActionResult Index(int id = 1)
        {
            int count;
            const int pageSize = 8;
            var result = _recentService.GetAllRecentActivitys();
            var model = result.ToPagedList(id, pageSize);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new RecentActivityDto();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(RecentActivityDto model)
        {
            if (!ModelState.IsValid) return View(model);
            _recentService.AddRecentActivity(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangeStatus(Int64 id)
        {
            var result = new QsResult();
            var model = _recentService.GetRecentActivityById(id);
            if (model == null)
            {
                result.Success = false;
                result.Message = @"找不到对象";
                return Json(result);
            }
            if (!model.Status)
            {
                result.Success = false;
                result.Message = @"该活动已经处于过去状态，修改状态失败";
                return Json(result);
            }
            model.Status = false;
            _recentService.ChangeRecentActivityDescription(id, model);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(Int64 id)
        {
            var result = new QsResult();
            var model = _recentService.GetRecentActivityById(id);
            if (model == null)
            {
                result.Success = false;
                result.Message = @"找不到对象";
                return Json(result);
            }
            _recentService.DeleteRecentActivity(id);
            return Json(result);
        }
    }
}
