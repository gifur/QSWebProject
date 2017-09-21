using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using LitJson;
using QS.Common;
using QS.Service;
using QS.Service.Effection;
using QS.Web.Tools;

namespace QS.Web.Areas.Admin.Common
{
    /// <summary>
    /// UploadImageHandler 的摘要说明
    /// </summary>
    public class UploadImageHandler : Controller, IHttpHandler, IRequiresSessionState
    {
        private HttpContext _context;
        private readonly IPhotoService _photoService;

        public UploadImageHandler()
        {
        }

        public UploadImageHandler(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public void ProcessRequest(HttpContext context)
        {
            var action = System.Web.HttpContext.Current.Request.QueryString["action"];
            switch (action)
            {
                case "SingleFile":
                    SingleFile(context);
                    break;
                case "MultipleFile":
                    MultipleFile(context);
                    break;
                default: //普通上传
                    UpLoadFile(context);
                    break;
            }
        }

        private void UpLoadFile(HttpContext context)
        {
            var delfile = System.Web.HttpContext.Current.Request.QueryString["DelFilePath"];
            var upfile = context.Request.Files["Filedata"];


            bool isWater = false; //默认不打水印
            bool isThumbnail = false; //默认不生成缩略图
            bool isImage = false;

            if (System.Web.HttpContext.Current.Request.QueryString["IsWater"] == "1")
                isWater = true;
            if (System.Web.HttpContext.Current.Request.QueryString["IsThumbnail"] == "1")
                isThumbnail = true;
            if (System.Web.HttpContext.Current.Request.QueryString["IsImage"] == "1")
                isImage = true;

            if (upfile == null)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"请选择要上传文件！\"}");
                return;
            }

            var upload = new UploadUtility();
            var result = upload.FileSaveAs(upfile, isThumbnail, isWater, isImage);
            if (!string.IsNullOrEmpty(delfile))
                Utilities.DeleteUpFile(delfile);
            context.Response.Write(result);
            context.Response.End();
        }

        private void EditorFile(HttpContext context)
        {
            var iswater = context.Request.QueryString["IsWater"] == "1";
            var imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                ShowError(context, "请选择要上传文件！");
                return;
            }
            var upload = new UploadUtility();
            string remsg = upload.FileSaveAs(imgFile, false, iswater);
            JsonData jd = JsonMapper.ToObject(remsg);
            string status = jd["status"].ToString();
            string msg = jd["msg"].ToString();
            if (status == "0")
            {
                ShowError(context, msg);
                return;
            }
            string filePath = jd["path"].ToString(); //取得上传后的路径
            var hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = filePath;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        private void SingleFile(HttpContext context)
        {
            string refilepath = System.Web.HttpContext.Current.Request.QueryString["ReFilePath"];  //取得返回的对象名称
            string upfilepath = System.Web.HttpContext.Current.Request.QueryString["UpFilePath"];  //取得上传的对象名称
            string delfile = System.Web.HttpContext.Current.Request.Form[refilepath];
            HttpPostedFile upfile = context.Request.Files[upfilepath];


            bool isWater = false; //默认不打水印
            bool isThumbnail = false; //默认不生成缩略图
            bool isImage = false;

            if (System.Web.HttpContext.Current.Request.QueryString["IsWater"] == "1")
                isWater = true;
            if (System.Web.HttpContext.Current.Request.QueryString["IsThumbnail"] == "1")
                isThumbnail = true;
            if (System.Web.HttpContext.Current.Request.QueryString["IsImage"] == "1")
                isImage = true;

            if (upfile == null)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"请选择要上传文件！\"}");
                return;
            }

            var upload = new UploadUtility();
            var result = upload.FileSaveAs(upfile, isThumbnail, isWater, isImage);
            //删除已存在的旧文件
            Utilities.DeleteUpFile(delfile);
            //返回成功信息
            context.Response.Write(result);
            context.Response.End();
        }

        private void MultipleFile(HttpContext context)
        {
            var upFilePath = context.Request.QueryString["UpFilePath"];
            var upfile = context.Request.Files[upFilePath];
            var isWater = false;
            var isThumbnail = false;

            if (context.Request.QueryString["IsWater"] == "1")
                isWater = true;
            if (context.Request.QueryString["IsThumbnail"] == "1")
                isThumbnail = true;

            if (upfile == null)
            {
                context.Response.Write("{\"msg\": 0, \"msgbox\": \"请选择要上传文件！\"}");
                return;
            }

            var upload = new UploadUtility();
            var result = upload.FileSaveAs(upfile, isThumbnail, isWater, true);
            context.Response.Write(result);
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