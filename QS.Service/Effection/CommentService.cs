using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.CommentAggregate;
using QS.DTO.CommentModule;
using QS.DTO.SharedModule;

namespace QS.Service.Effection
{

    public class CommentService : ICommentService
    {
        private readonly INewsCommentRepository _newsRepository;
        private readonly IArticleCommentRepository _articleRepository;
        private readonly IBookCommentRepository _bookRepository;
        private readonly IVideoCommentRepository _videoRepository;
        private readonly IUserRepository _userRepository;
        public CommentService() { }

        public CommentService(
            INewsCommentRepository newsRepository,
            IArticleCommentRepository articleRepository,
            IBookCommentRepository bookRepository,
            IVideoCommentRepository videoRepository,
            IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _newsRepository = newsRepository;
            _bookRepository = bookRepository;
            _videoRepository = videoRepository;
            _userRepository = userRepository;
        }

        #region 新闻评论
        public void AddNewsComment(NewsCommentDto commentDto)
        {
            _newsRepository.Add(QsMapper.CreateMap<NewsCommentDto, NewsComment>(commentDto));
            _newsRepository.UnitOfWork.Commit();
            IncreaseCommentNumsOf("News", commentDto.NewsId);
        }

        public void DeleteNewsComment(Int64 commentId)
        {
            var temp = _newsRepository.Get(commentId);
            if (temp == null) return;
            _newsRepository.Remove(temp);
            _newsRepository.UnitOfWork.Commit();
        }

        public NewsCommentDto GetNewestCommentInNewsWithFilter(string uniqueKey)
        {
            var temp = _newsRepository.FirstOrDefault(nc => nc.UniqueKey.Equals(uniqueKey));
            var result = QsMapper.CreateMap<NewsComment, NewsCommentDto>(temp);
            if (result == null || result.IsMember <= 0) return result;
            var user = _userRepository.Get(result.IsMember);
            result.NickName = user.UserName;
            result.PhotoUrl = user.PhotoUrl;
            result.Identification = user.Identification;
            return result;
        }
        public IEnumerable<NewsCommentDto> GetNewsCommentsWithTopic(Int64 id, out int count)
        {
            var results = _newsRepository.GetFiltered(nc => nc.NewsId == id, out count);
            results = results.OrderByDescending(nc => nc.CreateTime);
            var thenResults = QsMapper.CreateMapIEnume<NewsComment, NewsCommentDto>(results);
            foreach (var item in thenResults)
            {
                if (item.IsMember == 0) continue;
                var user = _userRepository.Get(item.IsMember);
                item.NickName = user.UserName;
                item.PhotoUrl = user.PhotoUrl;
                item.Identification = user.Identification;
            }
            return thenResults;
        }

        #endregion 结束新闻评论

        #region 文章评论

        public void AddArticleComment(ArticleCommentDto commentDto)
        {
            _articleRepository.Add(QsMapper.CreateMap<ArticleCommentDto, ArticleComment>(commentDto));
            _articleRepository.UnitOfWork.Commit();
            IncreaseCommentNumsOf("Article", commentDto.ArticleId);
        }

        public void DeleteArticleComment(long commentId)
        {
            var temp = _articleRepository.Get(commentId);
            if (temp == null) return;
            _articleRepository.Remove(temp);
            _articleRepository.UnitOfWork.Commit();
        }

        public ArticleCommentDto GetNewestCommentInArticleWithFilter(string uniqueKey)
        {
            var temp = _articleRepository.FirstOrDefault(nc => nc.UniqueKey.Equals(uniqueKey));
            var result = QsMapper.CreateMap<ArticleComment, ArticleCommentDto>(temp);
            if (result == null || result.IsMember <= 0) return result;
            var user = _userRepository.Get(result.IsMember);
            result.NickName = user.UserName;
            result.PhotoUrl = user.PhotoUrl;
            result.Identification = user.Identification;
            return result;
        }

        public IEnumerable<ArticleCommentDto> GetArticleCommentsWithTopic(long id, out int count)
        {
            var results = _articleRepository.GetFiltered(nc => nc.ArticleId == id, out count);
            results = results.OrderByDescending(nc => nc.CreateTime);
            var thenResults = QsMapper.CreateMapIEnume<ArticleComment, ArticleCommentDto>(results);
            if (thenResults == null) return null;
            foreach (var item in thenResults)
            {
                if (item.IsMember == 0) continue;
                var user = _userRepository.Get(item.IsMember);
                item.NickName = user.UserName;
                item.PhotoUrl = user.PhotoUrl;
                item.Identification = user.Identification;
            }
            return thenResults;
        }

        public IEnumerable<ArticleCommentDto> GetSecondsComments(int number)
        {
            if (number <= 0) number = 3;
            var results = _articleRepository.GetPaged(0, number, com => com.CreateTime, false);
            return QsMapper.CreateMapIEnume<ArticleComment, ArticleCommentDto>(results);
        }

        #endregion 结束文章评论

        #region 书籍评论

        public void AddBookComment(BookCommentDto commentDto)
        {
            _bookRepository.Add(QsMapper.CreateMap<BookCommentDto, BookComment>(commentDto));
            _bookRepository.UnitOfWork.Commit();
            IncreaseCommentNumsOf("Book", commentDto.BookId);
            var sb = new StringBuilder();
            var type = string.Empty;
            switch (commentDto.ReadStatus)
            {
                case (ReadStatus.Wish):
                {
                    break;
                }
                case (ReadStatus.Reading):
                {
                    break;
                }
                case (ReadStatus.Already):
                {
                    break;
                }
            }
            if (commentDto.ReadStatus != ReadStatus.Wish && commentDto.Score > 0)
            {
                
            }
        }

        public void DeleteBookComment(long commentId)
        {
            var temp = _bookRepository.Get(commentId);
            if (temp == null) return;
            _bookRepository.Remove(temp);
            _bookRepository.UnitOfWork.Commit();
        }

        public BookCommentDto GetNewestCommentInBookWithFilter(string uniqueKey)
        {
            var temp = _bookRepository.FirstOrDefault(nc => nc.UniqueKey.Equals(uniqueKey));
            var result = QsMapper.CreateMap<BookComment, BookCommentDto>(temp);
            if (result == null || result.IsMember <= 0) return result;
            var user = _userRepository.Get(result.IsMember);
            result.NickName = user.UserName;
            result.PhotoUrl = user.PhotoUrl;
            result.Identification = user.Identification;
            return result;
        }

        public IEnumerable<BookCommentDto> GetBookCommentsWithTopic(long id, out int count)
        {
            var results = _bookRepository.GetFiltered(nc => nc.BookId == id, out count);
            results = results.OrderByDescending(nc => nc.CreateTime);
            var thenResults = QsMapper.CreateMapIEnume<BookComment, BookCommentDto>(results);
            if (thenResults == null) return null;
            foreach (var item in thenResults)
            {
                if (item.IsMember == 0) continue;
                var user = _userRepository.Get(item.IsMember);
                item.NickName = user.UserName;
                item.PhotoUrl = user.PhotoUrl;
                item.Identification = user.Identification;
            }
            return thenResults;
        }

        public IEnumerable<BookCommentDto> GetSecondsBookComments(int number)
        {
            if (number <= 0) number = 3;
            var results = _bookRepository.GetPaged(0, number, com => com.CreateTime, false);
            return QsMapper.CreateMapIEnume<BookComment, BookCommentDto>(results);
        }
        #endregion 结束书籍评论

        #region 视频评论

        public void AddVideoComment(VideoCommentDto commentDto)
        {
            _videoRepository.Add(QsMapper.CreateMap<VideoCommentDto, VideoComment>(commentDto));
            _videoRepository.UnitOfWork.Commit();
            IncreaseCommentNumsOf("Video", commentDto.VideoId);
        }

        public void DeleteVideoComment(long commentId)
        {
            var temp = _videoRepository.Get(commentId);
            if (temp == null) return;
            _videoRepository.Remove(temp);
            _videoRepository.UnitOfWork.Commit();
        }

        public VideoCommentDto GetNewestCommentInVideoWithFilter(string uniqueKey)
        {
            var temp = _videoRepository.FirstOrDefault(nc => nc.UniqueKey.Equals(uniqueKey));
            var result = QsMapper.CreateMap<VideoComment, VideoCommentDto>(temp);
            if (result == null || result.IsMember <= 0) return result;
            var user = _userRepository.Get(result.IsMember);
            result.NickName = user.UserName;
            result.PhotoUrl = user.PhotoUrl;
            result.Identification = user.Identification;
            return result;
        }

        public IEnumerable<VideoCommentDto> GetVideoCommentsWithTopic(long id, out int count)
        {
            var results = _videoRepository.GetFiltered(nc => nc.VideoId == id, out count);
            results = results.OrderByDescending(nc => nc.CreateTime);
            var thenResults = QsMapper.CreateMapIEnume<VideoComment, VideoCommentDto>(results);
            if (thenResults == null) return null;
            foreach (var item in thenResults)
            {
                if (item.IsMember == 0) continue;
                var user = _userRepository.Get(item.IsMember);
                item.NickName = user.UserName;
                item.PhotoUrl = user.PhotoUrl;
                item.Identification = user.Identification;
            }
            return thenResults;
        }

        public IEnumerable<VideoCommentDto> GetSecondsVideoComments(int number)
        {
            if (number <= 0) number = 3;
            var results = _videoRepository.GetPaged(0, number, com => com.CreateTime, false);
            return QsMapper.CreateMapIEnume<VideoComment, VideoCommentDto>(results);
        }
        #endregion 结束视频评论

        public int IncreaseCommentNumsOf(string item, long id)
        {
            var sql = String.Format("UPDATE {0} SET CommentNum = CommentNum + 1 WHERE {1} = {2}", item, item+"Id", id);
            return _articleRepository.ExecuteCommand(sql);
        }
        public void DeleteComment(long commentId)
        {
            throw new NotImplementedException();
        }
        public CommentDto GetCommentById(long commentId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CommentDto> GetCommentsWithTopic(long id, out int count)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CommentDto> GetCommentsWithTopic(Guid id, out int count)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CommentDto> GetCommentsWithTopic(long id, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CommentDto> GetCommentsWithTopic(Guid id, int pageIndex, int pageCount, out int count)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<AtlasDto> GetAllComments()
        {
            throw new NotImplementedException();
        }
    }
}
