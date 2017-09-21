using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QS.DTO.SharedModule;
using QS.Service;
using QS.Web.Models;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Controllers.Shared
{
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        public VideoController() { }

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public ActionResult Index()
        {
            var models = _videoService.GetAllVideos();
            return View(models);
        }

        public ActionResult SubIndex(string category)
        {
            int count;
            var result = _videoService.GetVideosWithCategory(category, 0, 12, out count);
            var model = new VideoViewModel()
            {
                Category = category,
                Count = count,
                Contents = result
            };
            switch (category)
            {
                case("心理短片"): { model.DivId = "psychology"; break; }
                case("感悟人生"): { model.DivId = "life"; break; }
                case ("TED专场"): { model.DivId = "ted"; break; }
                case ("心理DV剧"): { model.DivId = "dvju"; break; }
                case("其他精彩"): { model.DivId = "other"; break; }
            }
            return PartialView("_VideoPartial", model);
        }

        public ActionResult Category(string category, int id = 1)
        {
            if (String.IsNullOrEmpty(category))
            {
                return RedirectToAction("Index");
            }
            int count;
            var cache = _videoService.GetVideosWithCategory(category, out count);
            var models = cache.ToPagedList(id, 16);
            ViewBag.Count = count;
            ViewBag.Category = category;
            return View(models);
        }

        public ActionResult Item(Int64 id)
        {
            _videoService.IncreaseViewsOf(id);
            var model = _videoService.GetVideoById(id);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult _VideoCommendPartial()
        {
            int count;
            var models = _videoService.GetTheCommendVideos(3, out count);
            return PartialView(models);
        }

        [ChildActionOnly]
        public ActionResult _VideoClickMostPartial()
        {
            var models = _videoService.GetMostClickVideos(10);
            return PartialView(models);
        }

    }
}
