using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class SuggestionController : BaseController
    {
        private readonly ISuggestionService _suggestionService;
        public SuggestionController() { }

        public SuggestionController(ISuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }

        public ActionResult Index(int id = 1)
        {
            int count;
            const int pageSize = 8;
            var result = _suggestionService.GetAllSuggestions();
            var model = result.ToPagedList(id, pageSize);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Int64 id)
        {
            var result = new QsResult();
            var model = _suggestionService.GetSuggestionById(id);
            if (model == null)
            {
                result.Success = false;
                result.Message = @"找不到对象";
                return Json(result);
            }
            _suggestionService.DeleteSuggestion(id);
            return Json(result);
        }

    }
}
