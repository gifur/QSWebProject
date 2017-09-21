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
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController() { }

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public ActionResult Index()
        {
            var model = new BookViewModel();
            var results = _bookService.GetAllBooks().AsQueryable();
            var keys = model.CategoryDict.Keys.ToArray();
            foreach (var t in keys)
            {
                model.CategoryDict[t] = results.Count(book => book.Category.Contains(t));
            }
            model.FourNewestBooks = results.OrderByDescending(book => book.CreateTime).Take(4).ToList();
            return View(model);
        }

        public ActionResult Category(string category, int id = 1)
        {
            int count;
            var cache = _bookService.GetBooksWithCategory(category, out count);
            var model = cache.ToPagedList(id, 8);
            ViewBag.Category = category;
            return View(model);
        }

        public ActionResult Item(Int64 id)
        {
            _bookService.IncreaseViewsOfBookOf(id);
            var model = _bookService.GetBookById(id);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult _RandomBookPartial()
        {
            var models = _bookService.GetRandomBooks();
            return PartialView(models);
        }

        [ChildActionOnly]
        public ActionResult _MostWishBookPartial()
        {
            var models = _bookService.GetMostItemBooks("Wish");
            return PartialView(models);
        }

        [ChildActionOnly]
        public ActionResult _HighGradeBookPartial()
        {
            var models = _bookService.GetHighGradeBooks();
            return PartialView(models);
        }

        [ChildActionOnly]
        public ActionResult _MostReadBookPartial()
        {
            var models = _bookService.GetMostItemBooks("Already", 10);
            return PartialView(models);
        }

        public ActionResult MoreAvailableBook()
        {
            return RedirectToAction("Index", "SiteStatus");
            return View();
        }
    }
}
