using System;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace QS.Web.Common
{
    /// <summary>
    /// 错误日志（Controller发生异常时会执行这里）
    /// </summary>
    public class LogExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpException = filterContext.Exception as HttpException;
            var innerException = filterContext.Exception;

            if (filterContext.ExceptionHandled)
            {
                //MVC处理HttpException的时候，如果为500 则会自动将其ExceptionHandled设置为true，此时将无法捕获异常
                if (httpException != null && httpException.GetHttpCode() != 500)  
                {
                    return;
                }
            }
            if (httpException == null)
            {
                filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                filterContext.Result = new RedirectResult("/SiteStatus/Error");//跳转至错误提示页面
            }
            //如果请求为AJAX请求则返回 JSON
            else if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception
                    }
                };
            }
            else
            {
                filterContext.Controller.ViewBag.UrlRefer = filterContext.HttpContext.Request.UrlReferrer;
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        filterContext.Result = new RedirectResult("/SiteStatus/NotFound"); //跳转至404提示页面
                        break;
                    case 500:
                        filterContext.Result = new RedirectResult("/SiteStatus/InternalError");//跳转至500提示页面
                        break;
                    default:
                        filterContext.Result = new RedirectResult("/SiteStatus/Error");//跳转至错误提示页面
                        break;
                }
            }

            var error = innerException ?? new Exception("没更加详细的错误信息");
            var url = HttpContext.Current.Request.RawUrl;//错误发生地址
            //添加到日志文件中，如果可以的话，需要将url进行处理
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(url, error);
            filterContext.ExceptionHandled = true;
        }


        #region 获取IP
        /// <summary>
        /// 客户端ip(访问用户)
        /// </summary>
        public static string GetUserIp
        {
            get
            {
                var realRemoteIp = "";
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    realRemoteIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
                }
                if (string.IsNullOrEmpty(realRemoteIp))
                {
                    realRemoteIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(realRemoteIp))
                {
                    realRemoteIp = HttpContext.Current.Request.UserHostAddress;
                }
                return realRemoteIp;
            }
        }

        #endregion
    } 
}