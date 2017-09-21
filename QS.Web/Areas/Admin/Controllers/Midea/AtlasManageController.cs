using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.SharedModule;
using Webdiyer.WebControls.Mvc;
using QS.Service;
using QS.Web.Tools;

namespace QS.Web.Areas.Admin.Controllers.Midea
{
    public class AtlasManageController : BaseController
    {
        private readonly IAtlasService _atlasService;
        private readonly IPhotoService _photoService;

        public AtlasManageController() { }

        public AtlasManageController(IAtlasService atlasService, IPhotoService photoService)
        {
            _atlasService = atlasService;
            _photoService = photoService;
        }

        public ActionResult Index(int id = 1)
        {
            var cache = _atlasService.GetAllAtlases();
            var model = cache.ToPagedList(id, 8);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string AtlasName, string Remark)
        {
            var model = new AtlasDto();
            if (!String.IsNullOrEmpty(AtlasName) && !String.IsNullOrEmpty(Remark))
            {
                model.AtlasId = Utilities.NewComb();
                model.AtlasName = AtlasName;
                model.Remark = Remark;
                _atlasService.AddAtlas(model);
                return Json(new {status = "success", data = model.AtlasId});
            }
            return Json(new {status = "error"});
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Receive(HttpPostedFileBase fileData, string atlasId)
        {
            var remark = String.Empty;
            if (String.IsNullOrEmpty(atlasId))
            {
                remark = @"图册编号为空，请确认后再提交";
               var data = new { state = false, description = remark };
               return Json(data); 
            }
            if (fileData != null && fileData.ContentLength > 0)
            {
                var atlasGuid = new Guid(atlasId);
                var temp = _atlasService.GetAtlasById(atlasGuid);
                if (temp == null)
                {
                    remark = String.Format("未能根据编号[{0}]找到该分享图册", atlasId);
                    var data = new { state = false, description = remark };
                    return Json(data);
                }

                const bool isWater = false;
                const bool isThumbnail = true;
                var upload = new UploadUtility(StoreAreaForUpload.ForGallery);
                var photoDto = upload.PictureSaveAs(fileData, isThumbnail, isWater, false);
                if (photoDto.PhotoId == Guid.Empty)
                {
                    var data = new { state = false, description = photoDto.Remark };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

                photoDto.AtlasId = atlasGuid;
                photoDto.Remark = @"暂无描述...";
                _photoService.AddPhoto(photoDto);
                var result = new { state = true, item = photoDto };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            var error = new { state = false, description = @"无上传的图片" };
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用于在新建时保存图册封面图像地址
        /// </summary>
        /// <param name="atlasId"></param>
        /// <param name="photoId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(string atlasId, string photoId)
        {
            var atlasGuid = new Guid(atlasId);
            var atlasModel = _atlasService.GetAtlasById(atlasGuid);
            var result = String.Empty;
            if (atlasModel != null)
            {
                if (String.IsNullOrEmpty(photoId))
                {
                    const string noImgPath = @"/Areas/assets/img/no-cover.png";
                    result = @"无图片情况下修改封面成功";
                    atlasModel.ThumbPath = noImgPath;
                }
                else
                {
                    var photoModel = _photoService.GetPhotoById(new Guid(photoId));
                    atlasModel.ThumbPath = photoModel.ThumbPath;
                    result = @"修改封面成功： " + photoId;
                }

                _atlasService.ChangeAtlasDetail(atlasGuid, atlasModel);
            }
            if (Request.IsAjaxRequest())
            {
                var data = new { success = true, message = result };
                return Json(data);
            }
            return RedirectToAction("Detail", new { id = atlasGuid } );
        }

        [HttpPost]
        public ActionResult Delete(Guid atlasId)
        {
            var photoList = _photoService.GetPhotosUnderAtlasId(atlasId).ToArray();
            foreach (var item in photoList)
            {
                _photoService.DeletePhoto(item.PhotoId);
            }
            _atlasService.DeleteAtlas(atlasId);
            return Content("success");
        }

        [HttpPost]
        public ActionResult Edit(Guid pk, string name, string value)
        {
            var model = _atlasService.GetAtlasById(pk);
            if (model == null)
            {
                var data = new {success = false, message = @"不存在该对象"};
                return Json(data);
            }
            if (name.Equals("atlastitle"))
            {
                model.AtlasName = value;
                _atlasService.ChangeAtlasDetail(pk, model);
            }
            else if (name.Equals("description"))
            {
                model.Remark = value;
                _atlasService.ChangeAtlasDetail(pk, model);
            }
            else if (name.Equals("thumbpath"))
            {
                model.ThumbPath = value;
                _atlasService.ChangeAtlasDetail(pk, model);
            }
            return new EmptyResult();
        }

        public ActionResult Detail(Guid id)
        {
            //var id = new Guid("a66e06a7-d3ee-4d2c-8b2e-a4120116c419");
            var model = _atlasService.GetAtlasById(id);
            if (model == null) return new HttpNotFoundResult();
            var photoList = _photoService.GetPhotosUnderAtlasId(id);
            ViewBag.underPhotos = photoList;
            return View(model);
        }
    }
}
