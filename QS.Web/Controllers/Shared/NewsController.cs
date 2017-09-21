using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using QS.Common;
using QS.DTO.SharedModule;
using QS.Service;
using QS.Web.Models;

namespace QS.Web.Controllers.Shared
{
    public class NewsController : DefaultController
    {
        private readonly INewsService _newsService;

        public NewsController() { }

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public ActionResult Index(int pageIndex = 1)
        {
            const int pageSize = 5;
            int count;
            var result = _newsService.GetNewsPaged(pageIndex, pageSize, out count).ToList();
            var temp = DtoToModel(result);
            var model = new PagedList<NewsSummaryModel>(temp, pageIndex, pageSize, count);
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
            var result = _newsService.GetPagedWithCategory(category, id, pageSize, out count).ToList();
            var temp = DtoToModel(result);
            var model = new PagedList<NewsSummaryModel>(temp, id, pageSize, count);
            return View(model);
        }

        public ActionResult Item(Int64 id)
        {
            _newsService.IncreaseViewsOf(id);
            var model = _newsService.GetNewsById(id);
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult _HotHits(int number)
        {
            var results = _newsService.GetMostPopular(number);
            var temp = DtoToModel(results, 56);
            return PartialView(temp);
        }

        [ChildActionOnly]
        public ActionResult _RecentestNews()
        {
            int count;
            var result = _newsService.GetNewsPaged(1, 2, out count);
            var temp = DtoToModel(result, 160);
            return PartialView(temp);
        }
        
        #region 私有方法

        private static IEnumerable<NewsSummaryModel> DtoToModel(IEnumerable<NewsDto> origin, int leave = 240)
        {
            var newsView = new List<NewsSummaryModel>();
            const string noImage = @"/assets/img/bg/nonews_photo_400x300.jpg";
            foreach (var item in origin)
            {
                var temp = QsMapper.CreateMap<NewsDto, NewsSummaryModel>(item);
                temp.NewsContent = Utilities.DropHtml(item.NewsContent, leave);
                temp.ViewImage = !String.IsNullOrEmpty(item.ThumbPath) ? item.ThumbPath : Utilities.GetImgUrl(item.NewsContent);
                if (String.IsNullOrEmpty(temp.ViewImage))
                    temp.ViewImage = noImage;
                newsView.Add(temp);
            }
            return newsView;
        }
        #endregion

    }
}
