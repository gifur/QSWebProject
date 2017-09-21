using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.SharedAggregate;
using QS.DTO.SharedModule;

namespace QS.Service.Effection
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService() { }

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public void AddNews(NewsDto newsDto)
        {
            newsDto.CreateTime = DateTime.Now;
            _newsRepository.Add(QsMapper.CreateMap<NewsDto, News>(newsDto));
            _newsRepository.UnitOfWork.Commit();
        }

        public void DeleteNews(Int64 newsId)
        {
            var temp = _newsRepository.Get(newsId);
            if (temp != null)
            {
                _newsRepository.Remove(temp);
                _newsRepository.UnitOfWork.Commit();
            }
        }

        public NewsDto GetNewsById(Int64 newsId)
        {
            var temp = _newsRepository.Get(newsId);
            return temp == null ? null : (QsMapper.CreateMap<News, NewsDto>(temp));
        }

        public bool ChangeNewsDescription(Int64 newsId, NewsDto updatedNewsDto)
        {
   
            var original = _newsRepository.Get(newsId);
            var recent = QsMapper.CreateMap<NewsDto, News>(updatedNewsDto);
            if (original != null && recent != null)
            {
                _newsRepository.Merge(original, recent);
                _newsRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<NewsDto> GetAllNews()
        {
            var allNews = _newsRepository.GetAll().OrderByDescending(n => n.CreateTime).AsEnumerable();
            //var allNews = _newsRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<News, NewsDto>(allNews);
        }

        public IEnumerable<NewsDto> GetNewsPaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }    
            //var newsEnumrable = _newsRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            var newsEnumrable = _newsRepository.GetPaged(pageIndex, pageCount, out count, n => n.CreateTime, false);
            return QsMapper.CreateMapIEnume<News, NewsDto>(newsEnumrable);
        }

        public IEnumerable<NewsDto> GetPagedWithCategory(string category, int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            if (String.IsNullOrEmpty(category))
            {
                return GetNewsPaged(pageIndex, pageCount, out count);
            }

            //var newsEnumrable = _newsRepository.GetPagedWithFilter(filter => filter.Category.Equals(category), 
            //    pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            var newsEnumrable = _newsRepository.GetPagedWithFilter(filter => filter.Category.Equals(category),pageIndex-1, pageCount, out count,  n => n.CreateTime, false);
            return QsMapper.CreateMapIEnume<News, NewsDto>(newsEnumrable);
        }

        public IEnumerable<NewsDto> GetItemWithKeyword(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
                return null;
            var newsEnumrable = _newsRepository.GetFiltered(it => it.NewsTitle.Contains(keyword));
            return QsMapper.CreateMapIEnume<News, NewsDto>(newsEnumrable);
        }

        public IEnumerable<NewsDto> GetMostPopular(int number, bool onView = true)
        {
            if (number <= 0) number = 3;
            var results = _newsRepository.GetPaged(0, number, art => art.ViewTimes, false);
            return QsMapper.CreateMapIEnume<News, NewsDto>(results);
        }

        public int IncreaseViewsOf(long newsId)
        {
            var sql = String.Format("UPDATE News SET ViewTimes = ViewTimes + 1 WHERE NewsId = {0}", newsId);
            return _newsRepository.ExecuteCommand(sql);
        }
    }
}
