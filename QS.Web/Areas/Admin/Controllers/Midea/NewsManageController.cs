using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using QS.Web.Areas.Admin.Models.ModelBinders;
using QS.Web.Tools;
using Webdiyer.WebControls.Mvc;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.SharedModule;
using QS.Service;
using QS.Web.Areas.Admin.Models;
using System.Text.RegularExpressions;

namespace QS.Web.Areas.Admin.Controllers.Midea
{
    public class NewsManageController : BaseController
    {
        private readonly INewsService _newsService;

        const string SavePath = @"~/Attached/News/";

        public NewsManageController() { }
        public NewsManageController(INewsService newService)
        {
            _newsService = newService;
        }

        public ActionResult Index(int id = 1)
        {
            var cache = _newsService.GetAllNews();
            var temp = QsMapper.CreateMapIEnume<NewsDto, NewsViewModel>(cache);
            var model = temp.ToPagedList(id, 8);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult Create()
        {
            BindTagItem();
            var model = new NewsDto();
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(NewsDto model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageInput"];
                if (file != null && file.ContentLength > 0)
                {
                    var upload = new UploadUtility(StoreAreaForUpload.ForNews);
                    var result = upload.SharedCoverSaveAs(file);
                    if (!result.Success)
                    {
                        ModelState.AddModelError("UploadError", result.Message);
                        return View(model);
                    }
                    model.ThumbPath = result.Message;
                }
                else
                {
                    const string noImage = @"/assets/img/bg/nonews_photo_400x300.jpg";
                    model.ThumbPath = Utilities.GetImgUrl(model.NewsContent);
                    if (String.IsNullOrEmpty(model.ThumbPath))
                        model.ThumbPath = noImage;
                }
                _newsService.AddNews(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Detail(Int64 id)
        {
            var model = _newsService.GetNewsById(id);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Int64 pk, string name, string value)
        {
            var model = _newsService.GetNewsById(pk);
            var flag = false;
            if (name.Equals("newsTitle"))
            {
                flag = true;
                model.NewsTitle = value;
            }
            else if (name.Equals("top"))
            {
                if (value.Equals("1"))
                {
                    flag = true;
                    model.IsTop = true;
                }
                else if (value.Equals("0"))
                {
                    flag = true;
                    model.IsTop = false;
                }
            }
            else if (name.Equals("category"))
            {
                flag = true;
                model.Category = value;
            }
            else if (name.Equals("content"))
            {
                flag = true;
                model.NewsContent = value;
            }
            if (flag)
            {
                _newsService.ChangeNewsDescription(pk, model);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Edit2([ModelBinder(typeof(JsonBinder<EditTagItem>))]List<EditTagItem> tags)
        {
            var tag = tags[0];
            var model = _newsService.GetNewsById(tag.pk);
            var newsTags = String.Join(",", tag.value);
            model.NewsTags = newsTags;
            _newsService.ChangeNewsDescription(tag.pk, model);
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsContent(string content)
        {
            return PartialView("_PartialContent", content);
        }

        public ActionResult Delete(Int64 id)
        {
            _newsService.DeleteNews(id);
            return Content("删除成功");
        }

        public ActionResult Upload()
        {
            return BaseUpload(SavePath);
        }

        public ActionResult FileManager()
        {
            return BaseFileManager(SavePath);
        }

        private void BindTagItem()
        {
            var newsTags = new List<SelectListItem>
                {
                    (new SelectListItem {Text = @"前沿发现", Value = "0"}),
                    (new SelectListItem {Text = @"心理趣事", Value = "1"}),
                    (new SelectListItem {Text = @"榜样人生", Value = "2"}),
                    (new SelectListItem {Text = @"活动盛举", Value = "3"}),
                    (new SelectListItem {Text = @"官方公告", Value = "4"}),
                    (new SelectListItem {Text = @"真情学生", Value = "5"}),
                    (new SelectListItem {Text = @"心理征文", Value = "6"}),
                    (new SelectListItem {Text = @"户外拓展", Value = "7"}),
                    (new SelectListItem {Text = @"线上活动", Value = "8"}),
                    (new SelectListItem {Text = @"线下活动", Value = "9"})
                };
            ViewData["NewsTag"] = newsTags;
        }

        public class TagItem
        {
            public string value { get; set; }
            public string text { get; set; }
        }

        [JsonObject]
        public class EditTagItem
        {
            [JsonProperty("pk")]
            public int pk { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("value")]
            public List<string> value { get; set; }
        }
    }
}
