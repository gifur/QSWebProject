using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Service;
using QS.Web.Models;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Controllers.Shared
{
    public class SearchController : DefaultController
    {
        private readonly IArticleService _articleService;
        private readonly IBookService _bookService;
        private readonly IAtlasService _atlasService;
        private readonly INewsService _newsService;
        private readonly IVideoService _videoService;

        public SearchController() { }

        public SearchController(IArticleService articleService, IBookService bookService, IAtlasService atlasService,
            INewsService newsService, IVideoService videoService)
        {
            _articleService = articleService;
            _bookService = bookService;
            _atlasService = atlasService;
            _newsService = newsService;
            _videoService = videoService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string keyword, int pageIndex = 1)
        {
            if(String.IsNullOrWhiteSpace(keyword))
                return View();
            ViewBag.KeyWord = keyword;
            var result = new List<SearchItemModel>();
            var articleResult = _articleService.GetItemWithKeyword(keyword);
            var newsResult = _newsService.GetItemWithKeyword(keyword);
            if (articleResult != null)
            {
                result.AddRange(articleResult.Select(item => new SearchItemModel(item)));
            }
            if (newsResult != null)
            {
                result.AddRange(newsResult.Select(item => new SearchItemModel(item)));
            }
            var temp = result.OrderByDescending(it => it.CreateTime).ToPagedList(pageIndex, 10);
            return View(temp);
        }

    }
}
