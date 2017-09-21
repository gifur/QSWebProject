using System.Web;
using System.Web.Mvc;
using QS.Web.Common;

namespace QS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //自定义特定异常过滤器，属性包括要处理的异常类型，发生异常时显示的视图名称
            //filters.Add(new HandleErrorAttribute
            //{
            //    ExceptionType = typeof(System.Data.DataException),
            //    View = "CustomerError"
            //});
            //如果 web.config 有將 custom errors 設成 On，程式裡面也有使用 HandleErrorAttribute，
            //那在發生錯誤時，程式會自動導向到 Error.cshtml,而忽略掉 customErrors 所設定的 defaultRedirect 跟裡面的 <error statusCode="404" redirect="~/error/notfound"></error>
            /*
             * 如果 web.config 有將 custom errors 設成 On，
                程式裡面沒有使用 HandleErrorAttribute，那在發生錯誤時，才會導向到 
                customErrors 所設定的 defaultRedirect 或裡面的 
                <error statusCode="404" redirect="~/error/notfound"></error>
             */
            //filters.Add(new HandleErrorAttribute(), 2);
            //处理页面出错的情况
            //filters.Add(new LogExceptionFilterAttribute(), 1);
        }
    }
}