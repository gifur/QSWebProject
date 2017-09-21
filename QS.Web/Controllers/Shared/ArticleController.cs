using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Web.Common;
using QS.Web.Tools;
using Webdiyer.WebControls.Mvc;
using QS.Common;
using QS.DTO.SharedModule;
using QS.Service;
using QS.Web.Models;

namespace QS.Web.Controllers.Shared
{
    public class ArticleController : DefaultController
    {
        private readonly IArticleService _articleService;
        public ArticleController() { }
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ActionResult Index(int pageIndex = 1)
        {
            const int pageSize = 5;
            int count;
            var result = _articleService.GetArticlePaged(pageIndex, pageSize, out count).ToList();
            var temp = DtoToModel(result);
            var model = new PagedList<ArticleSummaryModel>(temp, pageIndex, pageSize, count);
            return View(model);
        }

        public ActionResult Category(string category, int id = 1)
        {
            if (String.IsNullOrEmpty(category))
            {
                return RedirectToAction("Index");
            }
            const int pageSize = 5;
            int count;
            ViewBag.Category = category;
            var result = _articleService.GetPagedWithCategory(category, id, pageSize, out count).ToList();
            var temp = DtoToModel(result);
            var model = new PagedList<ArticleSummaryModel>(temp, id, pageSize, count);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult _PopularTenArticle()
        {
            var models = _articleService.GetMostPopular(10);
            return PartialView(models);
        }

        public ActionResult _NewestArticle()
        {
            int count;
            var models = _articleService.GetArticlePaged(1, 3, out count);
            var temp = DtoToModel(models, 65);
            return PartialView(temp);
        }

        public ActionResult Item(Int64 id)
        {
            _articleService.IncreaseViewsOfArticleOf(id);
            var model = _articleService.GetArticleById(id);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult _CategoryPartial()
        {
            var allArticles = _articleService.GetAllArticles().AsQueryable();
            var categories = new List<ArticleCategoryModel>()
            {
                new ArticleCategoryModel
                {
                  CategoryTitle = "youth",
                  Count = allArticles.Count(dto => dto.Category.Contains("youth")),
                  ThemeClass = "green",
                  ThemeIcon = "fa-book"
                },
                new ArticleCategoryModel
                {
                  CategoryTitle = "wisdomlife",
                  Count = allArticles.Count(dto => dto.Category.Contains("wisdomlife")),
                  ThemeClass = "blue",
                  ThemeIcon = "fa-camera-retro"
                },
                new ArticleCategoryModel
                {
                  CategoryTitle = "psyche",
                  Count = allArticles.Count(dto => dto.Category.Contains("psyche")),
                  ThemeClass = "yellow",
                  ThemeIcon = "fa-headphones"
                },
                new ArticleCategoryModel
                {
                  CategoryTitle = "qiusuo",
                  Count = allArticles.Count(dto => dto.Category.Contains("qiusuo")),
                  ThemeClass = "red",
                  ThemeIcon = "fa-fire"
                }
            };
            return PartialView(categories);


        }

        
        #region 私有方法

        private static IEnumerable<ArticleSummaryModel> DtoToModel(IEnumerable<ArticleDto> origin, int leave = 250)
        {
            var articleView = new List<ArticleSummaryModel>();
            const string noImage = @"/assets/img/bg/noarticle_photo_400x300.jpg";
            foreach (var item in origin)
            {
                var temp = QsMapper.CreateMap<ArticleDto, ArticleSummaryModel>(item);
                temp.ArticleContent = Utilities.DropHtml(item.ArticleContent, leave);
                temp.ViewImage = !String.IsNullOrEmpty(item.ThumbPath) ? item.ThumbPath : Utilities.GetImgUrl(item.ArticleContent);
                if (String.IsNullOrEmpty(temp.ViewImage))
                    temp.ViewImage = noImage;
                articleView.Add(temp);
            }
            return articleView;
        }
        #endregion

    }
}
