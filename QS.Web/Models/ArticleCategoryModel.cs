using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    public class ArticleCategoryModel
    {
        public string ThemeClass { get; set; }
        public string ThemeIcon { get; set; }
        public string CategoryTitle { get; set; }
        public int Count { get; set; }
    }
}