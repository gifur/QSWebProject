using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;

namespace QS.Web.Areas.Admin.Common
{
    /// <summary>
    /// KindEditor 的辅助类
    /// </summary>
    public class UploadAjaxHandler : IHttpHandler
    {
        private HttpContext _context;
        public void ProcessRequest(HttpContext context)
        {
            var url = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);
            const string savePath = @"~/Attached/";
            var saveUrl = "/Attached/";

            //定义允许上传的文件扩展名
            var extTable = new Hashtable
            {
                {"image", "gif,jpg,jpeg,png,bmp"},
                {"flash", "swf,flv"},
                {"media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb"},
                {"file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2"}
            };

            const int maxSize = 1000000;
            _context = context;

            var imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                ShowError("请选择文件。");
            }


            String dirPath = context.Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                ShowError("上传目录不存在。");
            }

            String dirName = context.Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                ShowError("目录名不正确。");
            }

            if (imgFile != null)
            {
                var fileName = imgFile.FileName;
                var fileExt = Path.GetExtension(fileName).ToLower();

                if (imgFile.InputStream.Length > maxSize)
                {
                    ShowError("上传文件大小超过限制。");
                }

                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    ShowError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
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
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(JsonMapper.ToJson(hash));
            }
            context.Response.End();
        }


        private void ShowError(string message)
        {
            var hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            _context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            _context.Response.Write(JsonMapper.ToJson(hash));
            _context.Response.End();
        }

        private void ShowError(HttpContext context, string message)
        {
            var hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}