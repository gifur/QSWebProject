using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.CommentModule;
using QS.Service;
using QS.Web.Common;
using QS.Web.Models;
using Webdiyer.WebControls.Mvc;
using System.Web.Security;

namespace QS.Web.Controllers
{
    public class CommentController : DefaultController
    {
        private readonly ICommentService _commentService;
        private readonly IArticleService _articleService;
        private readonly IVideoService _videoService;
        public CommentController() { }

        public CommentController(
            ICommentService commentService, 
            IArticleService articleService,
            IVideoService videoService)
        {
            _commentService = commentService;
            _articleService = articleService;
            _videoService = videoService;
        }

        #region 新闻评论

        //[ChildActionOnly]
        public ActionResult _NewsPartial(Int64 id, int pageIndex = 1)
        {
            const int pageSize = 5;
            int count;
            if (pageIndex < 1) pageIndex = 1;
            var cache = _commentService.GetNewsCommentsWithTopic(id, out count);
            var pageMaxIndex = (int)Math.Ceiling(count * 1.0 / pageSize);
            if (pageIndex > pageMaxIndex) pageIndex = pageMaxIndex;
            var model = cache.ToPagedList(pageIndex, pageSize);

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageMax = pageMaxIndex;
            ViewBag.Count = count;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _NewsFormPartial(Int64 id)
        {
            var model = new NewsCommentDto {NewsId = id};
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated) return PartialView(model);
            var curUser = CustomAuthorizeAttribute.GetUser();
            model.IsMember = curUser.UserId;
            model.NickName = curUser.UserName;
            model.Email = curUser.Email;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult NewsFormPartial(NewsCommentDto model)
        {
            var result = new QsResult();
            if (!Request.IsAjaxRequest()) return new EmptyResult();
            model.CreateTime = DateTime.Now;
            model.UniqueKey = Utilities.GetRamCodeOnDate();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var curUser = CustomAuthorizeAttribute.GetUser();
                if (curUser.UserId == model.IsMember)
                {
                    if (ModelState.IsValid)
                    {
                        //result.Success = true;
                        //result.Message = @"用户登陆状态下验证成功";
                        _commentService.AddNewsComment(model);
                        var newModel = _commentService.GetNewestCommentInNewsWithFilter(model.UniqueKey);
                        return PartialView("_SegmentPartial", newModel);
                    }
                    result.Success = false;
                    result.Message = @"用户登陆状态下验证失败~~";
                    return Json(result);
                }
                result.Success = false;
                result.Message = @"用户的编号在客户端被修改，导致内容不一致";
                return Json(result);
            }
            if (ModelState.IsValid)
            {
                //result.Success = true;
                //result.Message = @"游客状态下验证成功";
                //return Json(result);
                if (String.IsNullOrEmpty(model.NickName))
                    model.NickName = @"[匿名用户]";
                _commentService.AddNewsComment(model);
                var newModel = _commentService.GetNewestCommentInNewsWithFilter(model.UniqueKey);
                return PartialView("_SegmentPartial", newModel);
            }
            result.Success = false;
            result.Message = @"游客状态下验证失败";
            return Json(result);
        }

        #endregion 结束新闻评论

        #region 文章评论

        public ActionResult _ArticlePartial(Int64 id, int pageIndex = 1)
        {
            const int pageSize = 5;
            int count;
            if (pageIndex < 1) pageIndex = 1;
            var cache = _commentService.GetArticleCommentsWithTopic(id, out count);
            var pageMaxIndex = (int)Math.Ceiling(count * 1.0 / pageSize);
            if (pageIndex > pageMaxIndex) pageIndex = pageMaxIndex;
            var model = cache.ToPagedList(pageIndex, pageSize);

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageMax = pageMaxIndex;
            ViewBag.Count = count;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _ArticleFormPartial(Int64 id)
        {
            var model = new ArticleCommentDto { ArticleId = id };
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated) return PartialView(model);
            var curUser = CustomAuthorizeAttribute.GetUser();
            model.IsMember = curUser.UserId;
            model.NickName = curUser.UserName;
            model.Email = curUser.Email;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ArticleFormPartial(ArticleCommentDto model)
        {
            var result = new QsResult();
            if (!Request.IsAjaxRequest()) return new EmptyResult();
            model.CreateTime = DateTime.Now;
            model.UniqueKey = Utilities.GetRamCodeOnDate();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var curUser = CustomAuthorizeAttribute.GetUser();
                if (curUser.UserId == model.IsMember)
                {
                    if (ModelState.IsValid)
                    {
                        //result.Success = true;
                        //result.Message = @"用户登陆状态下验证成功";
                        _commentService.AddArticleComment(model);
                        var newModel = _commentService.GetNewestCommentInArticleWithFilter(model.UniqueKey);
                        return PartialView("_SegmentPartial", newModel);
                    }
                    result.Success = false;
                    result.Message = @"用户登陆状态下验证失败~~";
                    return Json(result);
                }
                result.Success = false;
                result.Message = @"用户的编号在客户端被修改，导致内容不一致";
                return Json(result);
            }
            if (ModelState.IsValid)
            {
                //result.Success = true;
                //result.Message = @"游客状态下验证成功";
                //return Json(result);
                if (String.IsNullOrEmpty(model.NickName))
                    model.NickName = @"[匿名用户]";
                _commentService.AddArticleComment(model);
                var newModel = _commentService.GetNewestCommentInArticleWithFilter(model.UniqueKey);
                return PartialView("_SegmentPartial", newModel);
            }
            result.Success = false;
            result.Message = @"游客状态下验证失败";
            return Json(result);
        }

        
        /// <summary>
        /// 在文章评论表中获取三条评论内容
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult _RecentCommentFromArticle()
        {
            var results = _commentService.GetSecondsComments(3).ToList();
            if (results.Count <= 0) return new EmptyResult();
            var models = results.Select(item => new NewestCommentModel
            {
                Id = item.ArticleId, 
                Title = _articleService.GetArticleById(item.ArticleId).ArticleTitle, 
                Content = Utilities.CutString(item.Content, 70), 
                Time = item.CreateTime
            }).ToList();
            return PartialView(models);
        }

        #endregion 结束文章评论

        #region 视频评论
        public ActionResult _VideoPartial(Int64 id, int pageIndex = 1)
        {
            const int pageSize = 5;
            int count;
            if (pageIndex < 1) pageIndex = 1;
            var cache = _commentService.GetVideoCommentsWithTopic(id, out count);
            var pageMaxIndex = (int)Math.Ceiling(count * 1.0 / pageSize);
            if (pageIndex > pageMaxIndex) pageIndex = pageMaxIndex;
            var model = cache.ToPagedList(pageIndex, pageSize);

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageMax = pageMaxIndex;
            ViewBag.Count = count;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _VideoFormPartial(Int64 id)
        {
            var model = new VideoCommentDto { VideoId = id };
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated) return PartialView(model);
            var curUser = CustomAuthorizeAttribute.GetUser();
            model.IsMember = curUser.UserId;
            model.NickName = curUser.UserName;
            model.Email = curUser.Email;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult VideoFormPartial(VideoCommentDto model)
        {
            var result = new QsResult();
            if (!Request.IsAjaxRequest()) return new EmptyResult();
            model.CreateTime = DateTime.Now;
            model.UniqueKey = Utilities.GetRamCodeOnDate();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var curUser = CustomAuthorizeAttribute.GetUser();
                if (curUser.UserId == model.IsMember)
                {
                    if (ModelState.IsValid)
                    {
                        //result.Success = true;
                        //result.Message = @"用户登陆状态下验证成功";
                        _commentService.AddVideoComment(model);
                        var newModel = _commentService.GetNewestCommentInVideoWithFilter(model.UniqueKey);
                        return PartialView("_SegmentPartial", newModel);
                    }
                    result.Success = false;
                    result.Message = @"用户登陆状态下验证失败~~";
                    return Json(result);
                }
                result.Success = false;
                result.Message = @"用户的编号在客户端被修改，导致内容不一致";
                return Json(result);
            }
            if (ModelState.IsValid)
            {
                //result.Success = true;
                //result.Message = @"游客状态下验证成功";
                //return Json(result);
                if (String.IsNullOrEmpty(model.NickName))
                    model.NickName = @"[匿名用户]";
                _commentService.AddVideoComment(model);
                var newModel = _commentService.GetNewestCommentInVideoWithFilter(model.UniqueKey);
                return PartialView("_SegmentPartial", newModel);
            }
            result.Success = false;
            result.Message = @"游客状态下验证失败";
            return Json(result);
        }

        [ChildActionOnly]
        public ActionResult _RecentCommentFromVideo()
        {
            var results = _commentService.GetSecondsVideoComments(3).ToList();
            if (results.Count <= 0) return new EmptyResult();
            var models = results.Select(item => new NewestCommentModel
            {
                Id = item.VideoId,
                Title = _videoService.GetVideoById(item.VideoId).VideoName,
                Content = Utilities.CutString(item.Content, 70),
                Time = item.CreateTime
            }).ToList();
            return PartialView(models);
        }

        #endregion 结束视频评论

        #region 书籍评论
        public ActionResult _BookPartial(Int64 id, int pageIndex = 1)
        {
            const int pageSize = 5;
            int count;
            if (pageIndex < 1) pageIndex = 1;
            var cache = _commentService.GetBookCommentsWithTopic(id, out count);
            var pageMaxIndex = (int)Math.Ceiling(count * 1.0 / pageSize);
            if (pageIndex > pageMaxIndex) pageIndex = pageMaxIndex;
            var model = cache.ToPagedList(pageIndex, pageSize);

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageMax = pageMaxIndex;
            ViewBag.Count = count;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _BookFormPartial(Int64 id)
        {
            var model = new BookCommentDto { BookId = id };
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated) return PartialView(model);
            var curUser = CustomAuthorizeAttribute.GetUser();
            model.IsMember = curUser.UserId;
            model.NickName = curUser.UserName;
            model.Email = curUser.Email;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult BookFormPartial(BookCommentDto model)
        {
            var result = new QsResult();
            if (!Request.IsAjaxRequest()) return new EmptyResult();
            model.CreateTime = DateTime.Now;
            model.UniqueKey = Utilities.GetRamCodeOnDate();
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                var curUser = CustomAuthorizeAttribute.GetUser();
                if (curUser.UserId == model.IsMember)
                {
                    if (ModelState.IsValid)
                    {
                        //result.Success = true;
                        //result.Message = @"用户登陆状态下验证成功";
                        _commentService.AddBookComment(model);
                        var newModel = _commentService.GetNewestCommentInBookWithFilter(model.UniqueKey);
                        return PartialView("_BookSegmentPartial", newModel);
                    }
                    result.Success = false;
                    result.Message = @"用户登陆状态下验证失败~~";
                    return Json(result);
                }
                result.Success = false;
                result.Message = @"用户的编号在客户端被修改，导致内容不一致";
                return Json(result);
            }
            if (ModelState.IsValid)
            {
                //result.Success = true;
                //result.Message = @"游客状态下验证成功";
                //return Json(result);
                if (String.IsNullOrEmpty(model.NickName))
                    model.NickName = @"[匿名用户]";
                _commentService.AddBookComment(model);
                var newModel = _commentService.GetNewestCommentInBookWithFilter(model.UniqueKey);
                return PartialView("_BookSegmentPartial", newModel);
            }
            result.Success = false;
            result.Message = @"游客状态下验证失败";
            return Json(result);
        }

        #endregion 结束书籍评论

    }
}



