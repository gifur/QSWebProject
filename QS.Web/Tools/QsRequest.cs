using System;
using System.Web;
using System.Text.RegularExpressions;

namespace QS.Web.Tools
{
    /// <summary>
    /// Request操作类
    /// </summary>
    public class QsRequest
    {
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
                return "";

            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;
        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            var request = HttpContext.Current.Request;
            return !request.Url.IsDefaultPort ? string.Format("{0}:{1}", request.Url.Host, request.Url.Port) : request.Url.Host;
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 得到主机名
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }


        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] browserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            var curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (var i = 0; i < browserName.Length; i++)
            {
                if (curBrowser.IndexOf(browserName[i], StringComparison.Ordinal) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] searchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            var tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (var i = 0; i < searchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(searchEngine[i], StringComparison.Ordinal) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        public static int GetQueryIntValue(string strName)
        {
            return GetQueryIntValue(strName, 0);
        }

        /// <summary>
        /// 返回指定URL的参数值(Int型)
        /// </summary>
        /// <param name="strName">URL参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定URL的参数值</returns>
        public static int GetQueryIntValue(string strName, int defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                var obj = new Regex("\\d+");
                var objmach = obj.Match(HttpContext.Current.Request.QueryString[strName].ToString());
                return objmach.Success ? Convert.ToInt32(objmach.Value) : defaultvalue;
            }
        }

        /// <summary>
        /// 获取用户当前IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
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


        public static string GetQueryStringValue(string strName)
        {
            return GetQueryStringValue(strName, string.Empty);
        }

        /// <summary>
        /// 返回指定URL的参数值(String型)
        /// </summary>
        /// <param name="strName">URL参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定URL的参数值</returns>
        public static string GetQueryStringValue(string strName, string defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\w+");
                Match objmach = obj.Match(HttpContext.Current.Request.QueryString[strName].ToString());
                if (objmach.Success)
                    return objmach.Value;
                else
                    return defaultvalue;
            }
        }
        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            var urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        /// <summary>
        /// 返回指定表单的参数值(Int型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>返回指定表单的参数值(Int型)</returns>
        public static int GetFormIntValue(string strName)
        {
            return GetFormIntValue(strName, 0);
        }
        /// <summary>
        /// 返回指定表单的参数值(Int型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定表单的参数值</returns>
        public static int GetFormIntValue(string strName, int defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\d+");
                Match objmach = obj.Match(HttpContext.Current.Request.Form[strName].ToString());
                if (objmach.Success)
                    return Convert.ToInt32(objmach.Value);
                else
                    return defaultvalue;
            }
        }
        /// <summary>
        /// 返回指定表单的参数值(String型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>返回指定表单的参数值(String型)</returns>
        public static string GetFormStringValue(string strName)
        {
            return GetQueryStringValue(strName, string.Empty);
        }
        /// <summary>
        /// 返回指定表单的参数值(String型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定表单的参数值</returns>
        public static string GetFormStringValue(string strName, string defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\w+");
                Match objmach = obj.Match(HttpContext.Current.Request.Form[strName].ToString());
                if (objmach.Success)
                    return objmach.Value;
                else
                    return defaultvalue;
            }
        }



        public static string GetStringValue(string strName)
        {
            return GetStringValue(strName, string.Empty);
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="defaultvalue"></param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetStringValue(string strName, string defaultvalue)
        {
            if ("".Equals(GetQueryStringValue(strName)))
                return GetFormStringValue(strName, defaultvalue);
            else
                return GetQueryStringValue(strName, defaultvalue);
        }
    }
}