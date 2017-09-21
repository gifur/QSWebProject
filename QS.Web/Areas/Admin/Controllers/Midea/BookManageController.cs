using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Web.Tools;
using Webdiyer.WebControls.Mvc;
using QS.DTO.SharedModule;
using QS.Service;

namespace QS.Web.Areas.Admin.Controllers.Midea
{
    public class BookManageController : BaseController
    {
        private readonly IBookService _bookService;
        public BookManageController() { }

        public BookManageController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public ActionResult Index(int id = 1)
        {
            var cache = _bookService.GetAllBooks();
            var models = cache.ToPagedList(id, 8);
            return View(models);
        }

        public ActionResult Create()
        {
            var model = new BookDto();
            BindBookCategory(null);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(BookDto model)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageInput"];
                if (file != null && file.ContentLength > 0)
                {
                    var upload = new UploadUtility(StoreAreaForUpload.ForBook);
                    var result = upload.SharedCoverSaveAs(file);
                    if (!result.Success)
                    {
                        ModelState.AddModelError("UploadError", result.Message);
                        BindBookCategory(model.Category);
                        return View(model);
                    }
                    model.ThumbPath = result.Message;
                }
                _bookService.AddBook(model);
                return RedirectToAction("Index");
            }
            BindBookCategory(model.Category);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Int64 bookId)
        {
            _bookService.DeleteBook(bookId);
            return Content("true");
        }

        private void BindBookCategory(string selected)
        {
            var categories = new List<SelectListItem>
                {
                    (new SelectListItem {Text = @"心理科普", Value = "心理科普"}),
                    (new SelectListItem {Text = @"专业技术", Value = "专业技术"}),
                    (new SelectListItem {Text = @"经典著作", Value = "经典著作"}),
                    (new SelectListItem {Text = @"思想哲学", Value = "思想哲学"}),
                    (new SelectListItem {Text = @"励志人生", Value = "励志人生"})
                };
            if (!String.IsNullOrEmpty(selected))
            {
                categories.Find(v => v.Value.Equals(selected)).Selected = true;
            }
            ViewData["Category"] = categories;
        }

    }
}
