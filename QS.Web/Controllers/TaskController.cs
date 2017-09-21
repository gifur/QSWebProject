using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.FeedbackModule;
using QS.Service;
using QS.Web.Common;
using QS.Web.Models;
using QS.Web.Tools;

namespace QS.Web.Controllers
{
    [CustomAuthorize]
    public class TaskController : DefaultController
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IFbDocumentService _fbDocumentService;
        public TaskController() {}

        public TaskController(IFbDocumentService fbDocumentService, IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            _fbDocumentService = fbDocumentService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Feedback()
        {
            var model = new FeedbackViewModel();
            var current = _feedbackService.GetActiveItem();
            if (current == null) return View(model);
            model.Current = current;
            if (current.Status != EnumFbStatus.Underway) return View(model);
            var userDto = GetUserInCookie();
            if (userDto != null)
            {
                model.Record = _fbDocumentService.GetDocumentsUnderFeedback(current.FeedbackId)
                    .FirstOrDefault(fb => fb.UploaderId == userDto.UserId);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Feedback(FormCollection formCollection)
        {
            //复制上面的内容
            var model = new FeedbackViewModel();
            var current = _feedbackService.GetActiveItem();
            if (current == null)
            {
                return RedirectToAction("Feedback");
            }
            model.Current = current;
            var userDto = GetUserInCookie();
            if (current.Status != EnumFbStatus.Underway || userDto == null)
            {
                ModelState.AddModelError("duplicate", @"未授权或未登录导致不可上传");
                return View(model);
            }

            model.Record = _fbDocumentService.GetDocumentsUnderFeedback(current.FeedbackId)
                    .FirstOrDefault(fb => fb.UploaderId == userDto.UserId);


            var file = Request.Files["fileUrl"];
            if (file == null || file.ContentLength <= 0)
            {
                ModelState.AddModelError("duplicate", @"请选择上传的文件");
                return View(model);
            }

            var newFileName = file.FileName;
            //物理存储就要开始了
            var uploader = new UploadUtility(StoreAreaForUpload.ForFeedback, CustomFileType.File);
            var uniqueValue = userDto.StuNumber;
            if (model.Record != null)
            {
                var rad = new Random();
                var value = rad.Next(1000, 10000);
                uniqueValue += "_" + value;
            }
            var result = uploader.DocumentSaveAs(file, uniqueValue);
            if (!result.Success)
            {
                ModelState.AddModelError("duplicate", result.Message);
                return View(model);
            }

            var newDocument = new FbDocumentDto
            {
                DocumentId = Utilities.NewComb(),
                DocumentName = newFileName,
                DocumentUrl = result.Message,
                UploadDate = DateTime.Now,
                UploaderId = userDto.UserId,
                UploaderName = userDto.UserName,
                FeedbackId = current.FeedbackId
            };

            _fbDocumentService.AddDocument(newDocument);

            
            if (model.Record == null) return RedirectToAction("Feedback");

            //删除二次上传前的文件
            uploader.DeleteFileInPhysical(model.Record.DocumentUrl);
            _fbDocumentService.DeleteDocument(model.Record.DocumentId);

            return RedirectToAction("Feedback");
        }
    }
}
