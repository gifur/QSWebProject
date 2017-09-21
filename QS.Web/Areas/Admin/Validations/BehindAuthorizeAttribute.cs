using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using QS.Web.Models;

namespace QS.Web.Areas.Admin.Validations
{
    /// <summary>
    /// 设置后台登陆的权限 代码调用顺序为：OnAuthorization-->AuthorizeCore-->HandleUnauthorizedRequest
    /// </summary>
    public class BehindAuthorizeAttribute : AuthorizeAttribute
    {
        public new string[] Roles { get; set; }

        /// <summary>
        /// 提供一个入口点用于进行自定义授权检查
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            if (Roles == null || Roles.Length == 0)
            {
                return true;
            }

            var result =  Roles.Any(httpContext.User.IsInRole);
            if (!result)
            {
                httpContext.Response.StatusCode = 403;
            }
            return result;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                filterContext.Result = new RedirectResult("/Admin/OAuth/Login");
            }
        }

        /// <summary>
        /// 处理未能授权的 HTTP 请求。
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var path = filterContext.HttpContext.Request.Path;
            var routeValue = new RouteValueDictionary {
                { "Controller", "OAuth"}, 
                { "Action", "Login"},
                { "ReturnUrl", path}
            };

            filterContext.Result = new RedirectToRouteResult(routeValue);
        }

        public static UserSafetyModel GetUser()
        {
            if (!HttpContext.Current.Request.IsAuthenticated) return null;
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie
            if (authCookie == null) return null;
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密
            return ticket != null ? JsonConvert.DeserializeObject<UserSafetyModel>(ticket.UserData) : null;
        }
    }
}