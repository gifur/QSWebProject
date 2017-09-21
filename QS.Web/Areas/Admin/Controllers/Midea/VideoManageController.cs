using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.Web.Tools;
using Webdiyer.WebControls.Mvc;
using System.Web.Mvc;
using QS.DTO.SharedModule;
using QS.Service;

namespace QS.Web.Areas.Admin.Controllers.Midea
{
    public class VideoManageController : BaseController
    {
        private readonly IVideoService _videoService;
        public VideoManageController() { }
        public VideoManageController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public ActionResult Index(int id = 1)
        {
            var cache = _videoService.GetAllVideos();
            var model = cache.ToPagedList(id, 8);
            return View(model);
        }

        public ActionResult Create()
        {
            BindVideoCategory(null);
            var model = new VideoDto();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(VideoDto model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageInput"];
                if (file != null && file.ContentLength > 0)
                {
                    var upload = new UploadUtility(StoreAreaForUpload.ForVideo);
                    var result = upload.SharedCoverSaveAs(file);
                    if (!result.Success)
                    {
                        ModelState.AddModelError("UploadError", result.Message);
                        return View(model);
                    }
                    model.ThumbPath = result.Message;
                }
                _videoService.AddVideo(model);
                return RedirectToAction("Index");
            }
            BindVideoCategory(model.Category);
            return View(model);


        }

        [HttpPost]
        public ActionResult Delete(Int64 videoId)
        {
            _videoService.DeleteVideo(videoId);
            return Content("true");
        }

        private void BindVideoCategory(string selected)
        {
            var categories = new List<SelectListItem>
                {
                    (new SelectListItem {Text = @"心理短片", Value = "心理短片"}),
                    (new SelectListItem {Text = @"感悟人生", Value = "感悟人生"}),
                    (new SelectListItem {Text = @"TED专场", Value = "TED专场"}),
                    (new SelectListItem {Text = @"心理DV剧", Value = "心理DV剧"}),
                    (new SelectListItem {Text = @"其他精彩", Value = "其他精彩", Selected = true})
                };
            if (!String.IsNullOrEmpty(selected))
            {
                categories.Find(v => v.Value.Equals(selected)).Selected = true;
            }
            ViewData["Category"] = categories;
        }

    }
}
