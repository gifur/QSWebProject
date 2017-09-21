using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.CommentModule;
using QS.DTO.SharedModule;

namespace QS.Service
{
    public interface ICommentService
    {
        #region 新闻评论
        void AddNewsComment(NewsCommentDto commentDto);
        void DeleteNewsComment(Int64 commentId);
        NewsCommentDto GetNewestCommentInNewsWithFilter(string uniqueKey);
        IEnumerable<NewsCommentDto> GetNewsCommentsWithTopic(Int64 id, out int count);
        #endregion 结束新闻评论
        void DeleteComment(Int64 commentId);
        CommentDto GetCommentById(Int64 commentId);
        IEnumerable<CommentDto> GetCommentsWithTopic(Int64 id, out int count);
        IEnumerable<CommentDto> GetCommentsWithTopic(Guid id, out int count);
        IEnumerable<CommentDto> GetCommentsWithTopic(Int64 id, int pageIndex, int pageCount, out int count);
        IEnumerable<CommentDto> GetCommentsWithTopic(Guid id, int pageIndex, int pageCount, out int count);
        IEnumerable<AtlasDto> GetAllComments();

        #region 文章评论
        void AddArticleComment(ArticleCommentDto commentDto);
        void DeleteArticleComment(Int64 commentId);
        ArticleCommentDto GetNewestCommentInArticleWithFilter(string uniqueKey);
        IEnumerable<ArticleCommentDto> GetArticleCommentsWithTopic(Int64 id, out int count);

        IEnumerable<ArticleCommentDto> GetSecondsComments(int number);
        #endregion 结束文章评论

        #region 书籍评论
        void AddBookComment(BookCommentDto commentDto);
        void DeleteBookComment(Int64 commentId);
        BookCommentDto GetNewestCommentInBookWithFilter(string uniqueKey);
        IEnumerable<BookCommentDto> GetBookCommentsWithTopic(Int64 id, out int count);

        IEnumerable<BookCommentDto> GetSecondsBookComments(int number);
        #endregion 结束书籍评论

        #region 视频评论
        void AddVideoComment(VideoCommentDto commentDto);
        void DeleteVideoComment(Int64 commentId);
        VideoCommentDto GetNewestCommentInVideoWithFilter(string uniqueKey);
        IEnumerable<VideoCommentDto> GetVideoCommentsWithTopic(Int64 id, out int count);

        IEnumerable<VideoCommentDto> GetSecondsVideoComments(int number);
        #endregion 结束视频评论

        int IncreaseCommentNumsOf(string item, long id);
    }
}
