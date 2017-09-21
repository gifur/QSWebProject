using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface INewsService
    {
        void AddNews(NewsDto newsDto);
        void DeleteNews(Int64 newsId);
        NewsDto GetNewsById(Int64 newsId);
        bool ChangeNewsDescription(Int64 newsId, NewsDto updatedNewsDto);
        IEnumerable<NewsDto> GetAllNews();

        IEnumerable<NewsDto> GetNewsPaged(int pageIndex, int pageCount, out int count);

        IEnumerable<NewsDto> GetPagedWithCategory(string category, int pageIndex, int pageCount, out int count);
        IEnumerable<NewsDto> GetItemWithKeyword(string keyword);
        IEnumerable<NewsDto> GetMostPopular(int number, bool onView = true);
        int IncreaseViewsOf(Int64 newsId);
    }
}
