using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QS.DTO.SharedModule;
using QS.Common;

namespace QS.Web.Models
{
    public enum ResultItemType
    {
        Article = 0,
        Book,
        Atlas,
        News,
        Video
    }

    /// <summary>
    /// 用于搜索页面显示的模型类
    /// </summary>
    public class SearchItemModel
    {
        public string Title { get; set; }
        public string Context { get; set; }
        public Int64 IntLink { get; set; }
        public Guid GuidLing { get; set; }
        public ResultItemType Type { get; set; }
        public DateTime CreateTime { get; set; }

        public const int Length = 325;
        public SearchItemModel(ArticleDto articleDto)
        {
            Title = articleDto.ArticleTitle;
            Context = Utilities.DropHtml(articleDto.ArticleContent, Length);
            IntLink = articleDto.ArticleId;
            CreateTime = articleDto.CreateTime;
            Type = ResultItemType.Article;
        }
        public SearchItemModel(NewsDto newsDto)
        {
            Title = newsDto.NewsTitle;
            Context = Utilities.DropHtml(newsDto.NewsContent, Length);
            IntLink = newsDto.NewsId;
            CreateTime = newsDto.CreateTime;
            Type = ResultItemType.News;
        }
    }
}