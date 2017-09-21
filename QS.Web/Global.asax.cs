using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Castle.Windsor;
using Newtonsoft.Json;
using QS.Web.Dependency;
using QS.Web.Models;

namespace QS.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer _container;

        public MvcApplication()
        {
            //MvcApplication 应用程序启动时控制器的依赖注入容器的实现
            //Windsor IOC容器初始化
            this._container =
                new WindsorContainer().Install(new DependencyConventions());
            AuthorizeRequest += new EventHandler(MvcApplication_AuthorizeRequest);
        }

        public override void Dispose()
        {
            this._container.Dispose();
            base.Dispose();
        }
        protected void Application_Start()
        {
            //隐藏ASP.NET MVC 输出的版本编号
            MvcHandler.DisableMvcResponseHeader = true;

            //将新绑定器替换为默认模型绑定器，并处理没有指定绑定器的所有模型
            //ModelBinders.Binders.DefaultBinder = new JsonModelBinder();

            //读取日志应用程序一开始进行初始化配置
            //log4net.Config.XmlConfigurator.Configure();
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);



            //TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UserDto), typeof(UserMetadata)), typeof(UserMetadata));

            //DataAnnotationsModelValidatorProvider.RegisterAdapter(
            //    typeof(RemoteStuNumberAttribute),
            //    typeof(RemoteAttributeAdapter)
            //);

            //for Restfull Routing
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new RestfulRoutingRazorViewEngine());
            //RouteTable.Routes.MapRoutes<Routes>();
        }


        #region Firefox用的jQuery Uploadify设定 
        //详情请访问： http://www.it165.net/pro/html/201308/6899.html
        /// <summary>
        /// 处理Uploadify在FireFox下掉Session问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_BeginRequest(object sender, EventArgs e)
        {
            const string sessionParamName = "ASPSESSID";
            const string sessionCookieName = "ASP.NET_SessionId";
            if (HttpContext.Current.Request.Form[sessionParamName] != null)
            {
                UpdateCookie(sessionCookieName, HttpContext.Current.Request.Form[sessionParamName]);
            }
            else if (HttpContext.Current.Request.QueryString[sessionParamName] != null)
            {
                UpdateCookie(sessionCookieName, HttpContext.Current.Request.QueryString[sessionParamName]);
            }

            const string authParamName = "AUTHID";
            var authCookieName = FormsAuthentication.FormsCookieName;
            if (HttpContext.Current.Request.Form[authParamName] != null)
            {
                UpdateCookie(authCookieName, HttpContext.Current.Request.Form[authParamName]);
            }
            else if (HttpContext.Current.Request.QueryString[authParamName] != null)
            {
                UpdateCookie(authCookieName, HttpContext.Current.Request.QueryString[authParamName]);
            }
        }

        private static void UpdateCookie(string cookieName, string cookieValue)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (null == cookie)
            {
                cookie = new HttpCookie(cookieName);
            }
            cookie.Value = cookieValue;
            HttpContext.Current.Request.Cookies.Set(cookie);//重新設定cookie
        }

        #endregion

        public void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            /*
            if (HttpContext.Current.User == null) return;
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
            if (!(HttpContext.Current.User.Identity is FormsIdentity)) return;
            //Get current user identitied by forms
            var id = (FormsIdentity)HttpContext.Current.User.Identity;
            // get FormsAuthenticationTicket object
            var ticket = id.Ticket;
            var userData = ticket != null ? JsonConvert.DeserializeObject<UserSafetyModel>(ticket.UserData) : null;
            if (userData == null) return;
            var roles = userData.Roles.Split(',');
            // set the new identity for current user.
            HttpContext.Current.User = new GenericPrincipal(id, roles);
             */
            if (HttpContext.Current.User == null) return;
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return;
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            //从检索自 Forms 身份验证 Cookie 或 URL 的加密身份验证票证创建一个 FormsAuthenticationTicket 对象
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密
            var authUser = ticket != null ? JsonConvert.DeserializeObject<UserSafetyModel>(ticket.UserData) : null;
            if (authUser == null) return;
            var roles = authUser.Roles.Split(new[] { ',' });
            var id = new FormsIdentity(ticket);
            //重建HttpContext.Current.User，加入用户拥有的角色数组 
            var principal = new GenericPrincipal(id, roles);
            Context.User = principal;//存到HttpContext.User中
        }
    }
}