using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QS.Common;
using QS.Core.Module;
using QS.DTO.Module;
using QS.Web.Models;

namespace QS.Web.Common
{
    /// <summary>
    /// 权限验证
    /// </summary>
    public class CustomAuthorizeAttribute : ActionFilterAttribute 
    {
        /// <summary>
        /// 用户角色（未进行开发）
        /// </summary>
        public string Roles { get; set; }
        /// <summary>
        /// true表示需要将信息补充完整才能进行该动作
        /// </summary>
        public bool Required { get; set; }

        public CustomAuthorizeAttribute()
        {
            Roles = "";
            Required = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //如果不存在身份信息
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.Url == null) return;
                var returnUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
                var redirectUrl = string.Format("?ReturnUrl={0}", returnUrl);
                var loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
            else
            {
                var content = new ContentResult();
                //var user = GetUser();
                var id = (FormsIdentity)HttpContext.Current.User.Identity;
                var ticket = id.Ticket;
                var user = ticket != null ? JsonConvert.DeserializeObject<UserSafetyModel>(ticket.UserData) : null;
                if (user== null) return;
                //获取通过身份验证的身份
                var role = user.Roles;
                if (role.Contains(Roles))
                {
                    //如果验证通过了，如果用户并没有填写第一手资料，那么这里就需要一直跳转页面
                    if (user.State == UserState.Nonactivated && Required)
                    {
                        filterContext.HttpContext.Response.Redirect("/Account/Confirmation", true);
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/SiteStatus/Forbidden", true);
                }
                
            }
        }

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
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