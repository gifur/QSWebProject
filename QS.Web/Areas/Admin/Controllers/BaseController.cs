using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using QS.Web.Areas.Admin.Validations;
using QS.Web.Models;

namespace QS.Web.Areas.Admin.Controllers
{
    [BehindAuthorize(Roles=new[] {"Admin","Editor"})]
    public class BaseController : Controller
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
        /// <param name=></param>
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
        public ActionResult BaseUpload(string savePath)
        {
            var saveUrl = "/Attached/News/";
            //定义允许上传的文件扩展名
            var extTable = new Hashtable
            {
                {"image", "gif,jpg,jpeg,png,bmp"},
                {"flash", "swf,flv"},
                {"media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"},
                {"file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"}
            };
            const int maxSize = 1000000;
            var imgFile = Request.Files["imgFile"];
            if (imgFile == null)
            {
                return ShowError("请选择文件。");
            }
            var dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var dirName = Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return ShowError("目录名不正确。");
            }

            var fileName = imgFile.FileName;
            var extension = Path.GetExtension(fileName);
            if (extension == null)
            {
                return ShowError("后缀名为空。");
            }
            var fileExt = extension.ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return ShowError("上传文件大小超过限制。");
            }
            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return ShowError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            //创建文件夹
            dirPath += dirName + "/";
            saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            var fileUrl = saveUrl + newFileName;
            var hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            return Json(hash, "text/html;charset=UTF-8");
        }



        public ActionResult BaseFileManager(string savePath)
        {
            var rootUrl = savePath;
            //图片扩展名
            const string fileTypes = "gif,jpg,jpeg,png,bmp";

            string currentPath;
            string currentUrl;
            string currentDirPath;
            string moveupDirPath;

            var dirPath = Server.MapPath(savePath);
            var dirName = Request.QueryString["dir"];
            if (!String.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    return Content("Invalid Directory name.");
                }
                dirPath += dirName + "/";
                rootUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            //根据path参数，设置各路径和URL
            String path = Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return Content("不可访问");
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return Content("无效参数");
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                return Content("目录不存在");
            }

            //遍历目录取得文件信息
            var dirList = Directory.GetDirectories(currentPath);
            var fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            var result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            var dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            foreach (var t in dirList)
            {
                var dir = new DirectoryInfo(t);
                var hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            foreach (var t in fileList)
            {
                var file = new FileInfo(t);
                var hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            return Json(result, "text/html;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }
        private JsonResult ShowError(string message)
        {
            var hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            return Json(hash, "text/html;charset=UTF-8");
        }
        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                var xInfo = new FileInfo(x.ToString());
                var yInfo = new FileInfo(y.ToString());

                return String.CompareOrdinal(xInfo.FullName, yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                var xInfo = new FileInfo(x.ToString());
                var yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                var xInfo = new FileInfo(x.ToString());
                var yInfo = new FileInfo(y.ToString());

                return String.Compare(xInfo.Extension, yInfo.Extension, StringComparison.Ordinal);
            }
        }
    }
}
