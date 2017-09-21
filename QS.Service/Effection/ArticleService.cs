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
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService() { }

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public void AddArticle(ArticleDto articleDto)
        {
            articleDto.CreateTime = DateTime.Now;
            _articleRepository.Add(QsMapper.CreateMap<ArticleDto, Article>(articleDto));
            _articleRepository.UnitOfWork.Commit();
        }

        public void DeleteArticle(Int64 articleId)
        {
            var temp = _articleRepository.Get(articleId);
            if (temp != null)
            {
                _articleRepository.Remove(temp);
                _articleRepository.UnitOfWork.Commit();
            }
        }

        public ArticleDto GetArticleById(Int64 articleId)
        {
            var temp = _articleRepository.Get(articleId);
            return temp == null ? new ArticleDto() : (QsMapper.CreateMap<Article, ArticleDto>(temp));
        }

        public bool ChangeArticleDescription(Int64 articleId, ArticleDto updatedArticleDto)
        {
   
            var original = _articleRepository.Get(articleId);
            var recent = QsMapper.CreateMap<ArticleDto, Article>(updatedArticleDto);
            if (original != null && recent != null)
            {
                _articleRepository.Merge(original, recent);
                _articleRepository.UnitOfWork.Commit();
                return true;
            }
            return false;
        }

        public IEnumerable<ArticleDto> GetAllArticles()
        {
            var allArticle = _articleRepository.GetAll().OrderByDescending(n => n.CreateTime).AsEnumerable();
            //var allArticle = _articleRepository.GetAll().OrderByDescending(n => n.IsTop).ThenByDescending(n => n.CreateTime).AsEnumerable();
            return QsMapper.CreateMapIEnume<Article, ArticleDto>(allArticle);
        }

        public IEnumerable<ArticleDto> GetMostPopular(int number, bool onView = true)
        {
            if (number <= 0) number = 10;
            var results = _articleRepository.GetPaged(0, number, art => art.ViewTimes, false);
            return QsMapper.CreateMapIEnume<Article, ArticleDto>(results);
        }

        public IEnumerable<ArticleDto> GetArticlePaged(int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            var articleEnumrable = _articleRepository.GetPaged(pageIndex, pageCount, out count, n => n.CreateTime, false); 
            //var articleEnumrable = _articleRepository.GetPaged<Boolean, DateTime>(pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            return QsMapper.CreateMapIEnume<Article, ArticleDto>(articleEnumrable);
        }

        public IEnumerable<ArticleDto> GetItemWithKeyword(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
                return null;
            //var articleEnumrable = _articleRepository.GetFiltered(it => it.ArticleTitle.Contains(keyword) || it.ArticleContent.Contains(keyword));
            var articleEnumrable = _articleRepository.GetFiltered(it => it.ArticleTitle.Contains(keyword));
            return QsMapper.CreateMapIEnume<Article, ArticleDto>(articleEnumrable);
        }

        public IEnumerable<ArticleDto> GetPagedWithCategory(string category, int pageIndex, int pageCount, out int count)
        {
            if (pageIndex <= 0 || pageCount <= 0)
            {
                count = 0;
                return null;
            }
            if (String.IsNullOrEmpty(category))
            {
                return GetArticlePaged(pageIndex, pageCount, out count);
            }

            //var articleEnumrable = _articleRepository.GetPagedWithFilter(filter => filter.Category.Equals(category), pageIndex, pageCount, n => n.IsTop, n => n.CreateTime, false, out count);
            var articleEnumrable = _articleRepository.GetPagedWithFilter(filter => filter.Category.Equals(category), pageIndex-1, pageCount, out count, n => n.CreateTime, false);            
            return QsMapper.CreateMapIEnume<Article, ArticleDto>(articleEnumrable);
        }

        public int IncreaseViewsOfArticleOf(long articleId)
        {
            var sql = String.Format("UPDATE Article SET ViewTimes = ViewTimes + 1 WHERE ArticleId = {0}", articleId);
            return _articleRepository.ExecuteCommand(sql);
        }

        public int GetArticlesCountWithCategory(string category)
        {
            return _articleRepository.Count(art => art.Category.Contains(category));
        }
    }
}
