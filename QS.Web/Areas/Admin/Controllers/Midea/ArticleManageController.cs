using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QS.Web.Areas.Admin.Models.ModelBinders;
using QS.Web.Tools;
using Webdiyer.WebControls.Mvc;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.SharedModule;
using QS.Service;
using QS.Web.Areas.Admin.Models;

namespace QS.Web.Areas.Admin.Controllers.Midea
{
    public class ArticleManageController : BaseController
    {
        private readonly IArticleService _articleService;
        const string SavePath = @"~/Attached/News/";
        public ArticleManageController() { }
        public ArticleManageController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ActionResult Index(int id = 1)
        {
            var cache = _articleService.GetAllArticles();
            var temp = QsMapper.CreateMapIEnume<ArticleDto, ArticleViewModel>(cache);
            var model = temp.ToPagedList(id, 10);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult Create()
        {
            BindTagItem();
            var model = new ArticleDto();
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(ArticleDto model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageInput"];
                if (file != null && file.ContentLength > 0)
                {
                    var upload = new UploadUtility(StoreAreaForUpload.ForArticle);
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
                    const string noImage = @"/assets/img/bg/noarticle_photo_500x280.jpg";
                    model.ThumbPath = Utilities.GetImgUrl(model.ArticleContent);
                    if (String.IsNullOrEmpty(model.ThumbPath))
                        model.ThumbPath = noImage;
                }
                _articleService.AddArticle(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Detail(Int64 id)
        {
            var model = _articleService.GetArticleById(id);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Int64 pk, string name, string value)
        {
            var model = _articleService.GetArticleById(pk);
            var flag = false;
            if (name.Equals("articleTitle"))
            {
                flag = true;
                model.ArticleTitle = value;
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
                model.ArticleContent = value;
            }
            if (flag)
            {
                _articleService.ChangeArticleDescription(pk, model);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Edit2([ModelBinder(typeof(JsonBinder<EditTagItem>))]List<EditTagItem> tags)
        {
            var tag = tags[0];
            var model = _articleService.GetArticleById(tag.pk);
            var articleTags = String.Join(",", tag.value);
            model.ArticleTags = articleTags;
            _articleService.ChangeArticleDescription(tag.pk, model);
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ArticleContent(string content)
        {
            return PartialView("_PartialContent", content);
        }

        public ActionResult Delete(Int64 id)
        {
            _articleService.DeleteArticle(id);
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
            var articleTags = new List<SelectListItem>
                {
                    (new SelectListItem {Text = @"励志人生", Value = "0"}),
                    (new SelectListItem {Text = @"心理趣事", Value = "1"}),
                    (new SelectListItem {Text = @"青春文学", Value = "2"}),
                    (new SelectListItem {Text = @"人生感悟", Value = "3"}),
                    (new SelectListItem {Text = @"心理征文", Value = "4"}),
                    (new SelectListItem {Text = @"释然一笑", Value = "5"}),
                    (new SelectListItem {Text = @"治愈系列", Value = "6"})
                };
            ViewData["ArticleTag"] = articleTags;
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
