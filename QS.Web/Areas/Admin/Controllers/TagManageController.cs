using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.Module;
using QS.DTO.SharedModule;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class TagManageController : Controller
    {
        private readonly ITagService _tagService;
        public TagManageController() { }

        public TagManageController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public ActionResult Index(int pageIndex = 1)
        {
            var cache = _tagService.GetAllTags();
            var model = cache.ToPagedList(pageIndex, 8);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new TagDto();
            BindBelongItem();
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(TagDto model)
        {
            if (ModelState.IsValid)
            {
                model.TagSum = 0;
                _tagService.AddTag(model);
                return RedirectToAction("Index");
            }
            BindBelongItem();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = new QsResult();
            var model = _tagService.GetTagById(id);
            if (model == null)
            {
                result.Success = false;
                result.Message = @"找不到对象";
                return Json(result);
            }
            _tagService.DeleteTag(id);
            return Json(result);
        }

        private void BindBelongItem()
        {
            ViewData["BelongItems"] = new List<SelectListItem>
                {
                    (new SelectListItem {Text = @"所有媒介", Value = ((Int32)(TagBelongType.All)).ToString(CultureInfo.InvariantCulture), Selected = true}),
                    (new SelectListItem {Text = @"新闻速递", Value = ((Int32)(TagBelongType.News)).ToString(CultureInfo.InvariantCulture)}),
                    (new SelectListItem {Text = @"心理文章", Value = ((Int32)(TagBelongType.Article)).ToString(CultureInfo.InvariantCulture)}),
                    (new SelectListItem {Text = @"心理图片", Value = ((Int32)(TagBelongType.Gallery)).ToString(CultureInfo.InvariantCulture)}),
                    (new SelectListItem {Text = @"影视心灵", Value = ((Int32)(TagBelongType.Video)).ToString(CultureInfo.InvariantCulture)}),
                    (new SelectListItem {Text = @"书籍推荐", Value = ((Int32)(TagBelongType.Book)).ToString(CultureInfo.InvariantCulture)})
                };
        }
    }
}
