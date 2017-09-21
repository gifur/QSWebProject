using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.DTO.SharedModule;

namespace QS.Web.Models
{
        /*
        (new SelectListItem {Text = @"心理科普", Value = "心理科普"}),
        (new SelectListItem {Text = @"专业技术", Value = "专业技术"}),
        (new SelectListItem {Text = @"经典著作", Value = "经典著作"}),
        (new SelectListItem {Text = @"思想哲学", Value = "思想哲学"}),
        (new SelectListItem {Text = @"励志人生", Value = "励志人生"})
     */
    public class BookViewModel
    {
        public Dictionary<string, Int64> CategoryDict { get; set; }
        public List<BookDto> FourNewestBooks { get; set; }

        public BookViewModel()
        {
            CategoryDict = new Dictionary<string, long>(5)
            {
                {@"心理科普", 0},
                {@"励志人生", 0},
                {@"经典著作", 0},
                {@"思想哲学", 0},
                {@"专业技术", 0}
            };
            FourNewestBooks = new List<BookDto>(4);
        }
    }
}