using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.CommentModule;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISuggestionService _suggestionService;
        private readonly IRecentActivityService _recentService;
        public HomeController() { }

        public HomeController(ISuggestionService suggestionService, IRecentActivityService recentService)
        {
            _suggestionService = suggestionService;
            _recentService = recentService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _SuggestionFormPartial()
        {
            var model = new SuggestionDto();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult SuggestionFormPartial(SuggestionDto model)
        {
            if (!Request.IsAjaxRequest()) return new EmptyResult();
            var result = new QsResult();
            if (ModelState.IsValid)
            {
                _suggestionService.AddSuggestion(model);
                result.Success = true;
                return Json(result);
            }
            result.Success = false;
            result.Message = @"回传成功，验证失败";
            return Json(result);
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult Activity()
        {
            var cache = _recentService.GetAllRecentActivitys().Take(10);
            return View(cache);
        }
        [HttpPost]
        public ActionResult Activity(int skipCount)
        {
            var cache = _recentService.GetAllRecentActivitys().Skip(skipCount).Take(10);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ActivityPartial", cache);
            }
            return View(cache);
        }
    }
}
