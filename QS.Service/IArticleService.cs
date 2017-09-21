using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface IArticleService
    {
        void AddArticle(ArticleDto articleDto);
        void DeleteArticle(Int64 articleId);
        ArticleDto GetArticleById(Int64 articleId);
        bool ChangeArticleDescription(Int64 articleId, ArticleDto updatedArticleDto);
        IEnumerable<ArticleDto> GetAllArticles();
        int GetArticlesCountWithCategory(string category);
        IEnumerable<ArticleDto> GetMostPopular(int number, bool onView = true);
        IEnumerable<ArticleDto> GetArticlePaged(int pageIndex, int pageCount, out int count);
        IEnumerable<ArticleDto> GetItemWithKeyword(string keyword);
        IEnumerable<ArticleDto> GetPagedWithCategory(string category, int pageIndex, int pageCount, out int count);

        int IncreaseViewsOfArticleOf(Int64 articleId);
    }
}
