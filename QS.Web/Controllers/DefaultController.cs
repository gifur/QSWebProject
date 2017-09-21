using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using QS.Web.Models;

namespace QS.Web.Controllers
{
    public class DefaultController : Controller
    {
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    // 此处进行异常记录，可以记录到数据库或文本，也可以使用其他日志记录组件。
        //    // 通过filterContext.Exception来获取这个异常。
        //    const string filePath = @"D:\Temp\Exceptions.txt";
        //    var sw = System.IO.File.AppendText(filePath);
        //    sw.Write(filterContext.Exception.Message);
        //    sw.Close();
        //    // 执行基类中的OnException
        //    base.OnException(filterContext);

        //}

        /// <summary>
        /// 处理找不到控制器时所调用的函数
        /// </summary>
        /// <param name="actionName"></param>
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    Response.Redirect("");
        //}

        /// <summary>
        /// 将登录用户信息存储在Cookie中
        /// </summary>
        /// <param name="user">要存储的用户信息</param>
        /// <returns></returns>
        public void SetAuthCookie(UserSafetyModel user)
        {

            //为提供的用户名提供一个身份验证的票据
            FormsAuthentication.SetAuthCookie(user.UserName, true, FormsAuthentication.FormsCookiePath);
            //把用户对象保存在票据里
            var ticket = new FormsAuthenticationTicket(
                1, //版本
                user.UserName, //用户名连接Ticket名
                DateTime.Now, //cookie发行的本地日期和时间
                DateTime.Now.AddTicks(FormsAuthentication.Timeout.Ticks), //cookie终止的时间
                true,
                JsonConvert.SerializeObject(user));

            //加密票据 创建一个可存储在 Cookie 或 URL 中的字符串值
            var hashTicket = FormsAuthentication.Encrypt(ticket);
            //客户端js不需要读取到这个Cookie，所以最好设置HttpOnly=True，防止浏览器攻击窃取、伪造Cookie
            var userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket) {HttpOnly = true};

            //填写登陆Cookie
            Response.Cookies.Remove(userCookie.Name);
            Response.Cookies.Add(userCookie);

            //获取返回的Url
            //Response.Redirect(FormsAuthentication.GetRedirectUrl(user.UserName, user.RememberMe));
        }

        public void SafeOutAuthCookie()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            var cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "") {Expires = DateTime.Now.AddYears(-1)};
            Response.Cookies.Add(cookie1);
            var cookie2 = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(cookie2);
        }
        public static UserSafetyModel GetUserInCookie()
        {
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated) return null;
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie
            if (authCookie == null) return null;
            //从检索自 Forms 身份验证 Cookie 或 URL 的加密身份验证票证创建一个 FormsAuthenticationTicket 对象
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密
            return ticket != null ? JsonConvert.DeserializeObject<UserSafetyModel>(ticket.UserData) : null;
        }
    }
}
