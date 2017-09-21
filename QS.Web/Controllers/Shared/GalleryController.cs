using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Controllers.Shared
{
    public class GalleryController : Controller
    {
        private readonly IAtlasService _atlasService;
        private readonly IPhotoService _photoService;

        public GalleryController() { }

        public GalleryController(IAtlasService atlasService, IPhotoService photoService)
        {
            _atlasService = atlasService;
            _photoService = photoService;
        }

        public ActionResult Index()
        {
            var cache = _atlasService.GetAllAtlases().Take(20);
            return View(cache);
        }

        [HttpPost]
        public ActionResult Index(int skipCount)
        {
            var cache = _atlasService.GetAllAtlases().Skip(skipCount).Take(20);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GalleryPartial", cache);
            }
            return View(cache);
        }

        public ActionResult Index2()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _NewestGallery()
        {
            var model = _atlasService.GetNewestAtlases();
            return PartialView(model);
        }

        public ActionResult Detail(Guid id)
        {
            _atlasService.IncreaseViewsOf(id);
            var model = _atlasService.GetAtlasById(id);
            if (model == null) return new HttpNotFoundResult();
            var photoList = _photoService.GetPhotosUnderAtlasId(id);
            ViewBag.Photos = photoList;
            return View(model);
        }

        public ActionResult _PhotoGalleryPartial()
        {
            var models = _atlasService.GetRandomAtlases();
            return PartialView(models);
        }

        [ChildActionOnly]
        public ActionResult _PostPopularGallery()
        {
            var model = _atlasService.GetPopularAtlases();
            return PartialView(model);
        }

    }
}
