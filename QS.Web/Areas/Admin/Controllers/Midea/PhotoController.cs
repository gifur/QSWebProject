using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using QS.Service;
using QS.Web.Tools;

namespace QS.Web.Areas.Admin.Controllers.Midea
{
    public class PhotoController : BaseController
    {
        private readonly IPhotoService _photoService;

        public PhotoController()
        {

        }

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        //
        // GET: /Admin/Photo/

        public ActionResult Index(int id = 1)
        {
            var cache = _photoService.GetAllPhotos();
            var model = cache.ToPagedList(id, 8);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Guid pk, string name, string value)
        {
            var result = String.Empty;
            var model = _photoService.GetPhotoById(pk);
            if (name.Equals("remark"))
            {
                model.Remark = value;
                _photoService.ChangePhotoDetail(pk, model);
                result = @"修改图片描述成功";
            }
            if (Request.IsAjaxRequest())
            {
                var data = new { success = true, message = result };
                return Json(data);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Delete(Guid photoId)
        {
            var model = _photoService.GetPhotoById(photoId);
            var upload = new UploadUtility();
            //flag的大肆使用是想到后面优化时能记录到Log日志中
            var flag = true;
            if (!String.IsNullOrEmpty(model.PhotoPath))
            {
                flag = upload.DeleteFileInPhysical(model.PhotoPath);
                if (flag)
                {
                    if (!String.IsNullOrEmpty(model.ThumbPath))
                    {
                        flag = upload.DeleteFileInPhysical(model.ThumbPath);
                    }
                }
            }
            _photoService.DeletePhoto(photoId);
            return Content("true");
        }

        public ActionResult Upload()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Receive(HttpPostedFileBase fileData)
        {
            if (fileData != null && fileData.ContentLength > 0)
            {
                const bool isWater = false;
                const bool isThumbnail = true;
                var upload = new UploadUtility();
                var photoDto = upload.PictureSaveAs(fileData, isThumbnail, isWater, false);
                if (photoDto.PhotoId == Guid.Empty)
                {
                    var data = new { state = false, description = photoDto.Remark };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                _photoService.AddPhoto(photoDto);
                var result = new {state = true, item = photoDto};
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            var error = new {state = false, description = @"无上传的图片"};
            return Json(error, JsonRequestBehavior.AllowGet);
        }
    }
}
