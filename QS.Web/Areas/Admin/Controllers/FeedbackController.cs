using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.DTO.FeedbackModule;
using QS.Service;
using QS.Web.Areas.Admin.Models;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {

        private readonly IFeedbackService _feedbackService;
        private readonly IFbDocumentService _documentService;
        private const string OptionCreatesuccess = @"createSuccess";
        public FeedbackController(){}
        public FeedbackController(IFeedbackService feedbackService, IFbDocumentService documentService)
        {
            _feedbackService = feedbackService;
            _documentService = documentService;
        }

        public ActionResult Index()
        {
            var recentFeedback = _feedbackService.GetActiveItem();

            //说明可以开启新的心理反馈
            return recentFeedback == null ? RedirectToAction("History") : RedirectToAction("Current", new { id = recentFeedback.FeedbackId });
        }

        public ActionResult Current(int id)
        {
            var model = _feedbackService.GetFeedbackById(id);
            return View(model);
        }

        public ActionResult History()
        {
            var lst = _feedbackService.GetAllFeedback();
            return View(lst);
        }

        public ActionResult Detail(int feedbackId, string feedbackName)
        {
            var model = _feedbackService.GetFeedbackById(feedbackId);
            return View("Current", model);
        }

        public ActionResult Create()
        {
            var recentFeedback = _feedbackService.GetActiveItem();
            if (recentFeedback == null || recentFeedback.Status == EnumFbStatus.Closed)
            {
                //测试的时候放到下面去了，还没拉上来呢...
                //这两句
                var newFeedback = new FeedbackCreateModel();
                return View(newFeedback);
            }

            return Content("<script type='text/javascript'>alert('抱歉，因新的反馈处于待开始或进行中，导致现在不能创建！');history.go(-1);</script>");
        }

        [HttpPost]
        public ActionResult Create(FeedbackCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var recentFeedback = _feedbackService.GetActiveItem();
            if (recentFeedback == null || recentFeedback.Status == EnumFbStatus.Closed)
            {
                var feedbackDto = new FeedbackDto
                {
                    FeedbackName = model.Title,
                    StartTime = model.StartTime == null ? DateTime.Now : (DateTime)model.StartTime,
                    EndTime = model.EndTime == null ? DateTime.Now : (DateTime)model.EndTime
                };

                feedbackDto.JudgeStatus();
                _feedbackService.AddFeedback(feedbackDto);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult CurrentSearch(int parentId, int pageSize = 8)
        {
            var temp = _documentService.GetDocumentsUnderFeedback(parentId).AsQueryable();
            var model = temp.ToPagedList(1, pageSize);
            return PartialView("_PartialDocuments", model);
        }

        [HttpPost]
        public ActionResult CurrentSearch(string filter, int parentId, int pageIndex = 1, int pageSize = 8)
        {
            var temp = _documentService.GetDocumentsUnderFeedback(parentId).AsQueryable();
            if (!String.IsNullOrEmpty(filter))
            {
                temp = temp.Where(ft => ft.UploaderName.Contains(filter) || ft.DocumentName.Contains(filter));
            }
            var model = temp.ToPagedList(pageIndex, pageSize);
            if (!Request.IsAjaxRequest())
            {//必须排除这条路
                return new EmptyResult();
            }
            return PartialView("_PartialDocuments", model);
        }

        public ActionResult Download(Guid  id)
        {
            var temp = _documentService.GetDocumentById(id);
            if (temp != null)
            {
                var path = Server.MapPath(temp.DocumentUrl);
                return File(path, "text/plain", temp.DocumentName);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Close(int feedbackId)
        {
            var feedbackDto = _feedbackService.GetFeedbackById(feedbackId);
            feedbackDto.Status = EnumFbStatus.Closed;
            _feedbackService.UpdateFeedbackDetail(feedbackId, feedbackDto);
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult _RecentFeedbackBar()
        {
            var recentFeedback = _feedbackService.GetActiveItem();
            if (recentFeedback != null && recentFeedback.Status == EnumFbStatus.Underway)
            {
                var total = recentFeedback.EndTime.AddDays(1).Subtract(recentFeedback.StartTime).TotalHours;
                var recent = DateTime.Now.Subtract(recentFeedback.StartTime).TotalHours;
                ViewBag.Percent = (int)(recent/total*100);
            }
            return PartialView(recentFeedback);
        }
    }
}
