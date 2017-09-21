using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using QS.Common;
using QS.DTO.SharedModule;
using QS.Service;
using QS.Web.Common;

namespace QS.Web.Tools
{
    public class UploadUtility
    {
        private readonly CustomSiteConfig _siteConfig;

        public UploadUtility()
        {
            _siteConfig = new CustomSiteConfig(CustomFileType.Image);
        }

        public UploadUtility(StoreAreaForUpload area, CustomFileType fileType = CustomFileType.Image)
        {
            _siteConfig = new CustomSiteConfig(fileType);
            switch (area)
            {
                case(StoreAreaForUpload.ForVideo):
                {
                    _siteConfig.AttachPath =  _siteConfig.AttachPath + "/Videos";
                    _siteConfig.AttachImgMaxWidth = 180;
                    _siteConfig.AttachImgMaxHeight = 101;
                    break;
                }
                case(StoreAreaForUpload.ForGallery):
                {
                    _siteConfig.AttachPath += "/Gallery";
                    break;
                }
                case (StoreAreaForUpload.ForBook):
                {
                    _siteConfig.AttachPath += "/Books";
                    _siteConfig.ThumbnailWidth = 250;
                    _siteConfig.ThumbnailHeight = 360;
                    _siteConfig.AttachImgMaxWidth = 500;
                    _siteConfig.AttachImgMaxHeight = 720;
                    break;
                }
                case (StoreAreaForUpload.ForArticle):
                {
                    _siteConfig.AttachPath += "/Articles";
                    _siteConfig.AttachImgMaxWidth = 500;
                    _siteConfig.AttachImgMaxHeight = 280;
                    break;
                }
                case (StoreAreaForUpload.ForNews):
                {
                    _siteConfig.AttachPath += "/News";
                    _siteConfig.AttachImgMaxWidth = 400;
                    _siteConfig.AttachImgMaxHeight = 300;
                    break;
                }
                case (StoreAreaForUpload.ForFeedback):
                {
                    _siteConfig.AttachPath += "/Feedback";
                    _siteConfig.AttachSave = 3;
                    break;
                }
            }

        }

        /// <summary>
        /// 上传心理委员提交的心理反馈表
        /// </summary>
        /// <param name="postedFile">上传的文件</param>
        /// <param name="latter">上传用户的学号</param>
        /// <returns>返回原始的文件名，未包含学号</returns>
        public QsResult DocumentSaveAs(HttpPostedFileBase postedFile, string latter)
        {
            var result = new QsResult{Success = false };
            var fileType = Utilities.GetFileTypeName(postedFile.FileName);
            var fileSize = postedFile.ContentLength;
            var fileName =
                postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
            var newFileName = Utilities.GetFileNameWithoutType(fileName) + "_" + latter + "." + fileType;
            if (!CheckFileExt(fileType))
            {
                result.Message = @"不允许上传的文件类型";
                return result;
            }
            if (!CheckFileSize(fileSize))
            {
                result.Message = @"上传的文件大小超出限制的 " + _siteConfig.AttachImgSize;
                return result;
            }
            var dirPath = GetUpLoadPath();
            //存储至物理路径
            var toFileFullPath = Utilities.GetMapPath(dirPath);
            if (!Directory.Exists(toFileFullPath))
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            postedFile.SaveAs(toFileFullPath + newFileName);
            var serverFileName = dirPath + newFileName;
            result.Success = true;
            result.Message = serverFileName;
            return result;
        }

        public QsResult SharedCoverSaveAs(HttpPostedFileBase postedFile, bool isReOriginal = false)
        {
            var result = new QsResult();
            var fileType = Utilities.GetFileTypeName(postedFile.FileName);
            var fileSize = postedFile.ContentLength;
            var fileName =
                postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
            var newFileName = Utilities.GetRamCodeOnDate() + "." + fileType;
            if (isReOriginal)
            {
                //如果 isReOriginal为True，那就使用原始名字
                newFileName = Utilities.GetFileNameWithoutType(fileName) + "." + fileType;
            }
            if (!CheckFileExt(fileType))
            {
                result.Success = false;
                result.Message = @"不允许上传的文件类型";
                return result;
            }
            if (!IsImage(fileType))
            {
                result.Success = false;
                result.Message = @"检测到上传的是非图片文件";
                return result;
            }
            if (!CheckFileSize(fileSize))
            {
                result.Success = false;
                result.Message = @"上传的图片大小超出限制的 "+ _siteConfig.AttachImgSize;
                return result;
            }

            var dirPath = GetUpLoadPath();

            //存储至物理路径
            var toFileFullPath = Utilities.GetMapPath(dirPath);
            if (!Directory.Exists(toFileFullPath))
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            postedFile.SaveAs(toFileFullPath + newFileName);
            if (_siteConfig.AttachImgMaxHeight > 0 || _siteConfig.AttachImgMaxWidth > 0)
            {
                Thumbnail.MakeThumbnailImage(toFileFullPath + newFileName, toFileFullPath + newFileName, _siteConfig.AttachImgMaxWidth, _siteConfig.AttachImgMaxHeight);
            }
            var serverFileName = dirPath + newFileName;
            result.Message = serverFileName;
            return result;
        }

        /// <summary>
        /// 用于图片分享保存文件内容
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="isThumbnail"></param>
        /// <param name="isWater"></param>
        /// <param name="isReOriginal"></param>
        /// <returns>返回PhotoType文件类型</returns>
        public PhotoDto PictureSaveAs(HttpPostedFileBase postedFile, bool isThumbnail, bool isWater, bool isReOriginal)
        {
            var newPhoto = new PhotoDto();
            var fileType = Utilities.GetFileTypeName(postedFile.FileName);
            if (!CheckFileExt(fileType) || !IsImage(fileType))
            {
                newPhoto.PhotoId = Guid.Empty;
                newPhoto.Remark = @"文件类型为非图片类型";
                return newPhoto;

            }
            var fileSize = postedFile.ContentLength;
            var fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
            var temp = Utilities.GetRamCodeOnDate();
            var newFileName = temp + "." + fileType;
            if (isReOriginal)
            {
                newFileName = fileName;
            }
            var dirPath = GetUpLoadPath();

            if (!CheckFileSize(fileSize))
            {
                newPhoto.PhotoId = Guid.Empty;
                newPhoto.Remark = @"文件大小超出最高限制";
            }

            var serverFileName = dirPath + newFileName;
            var serverThumbnailFileName = dirPath + "thumb_" + newFileName;
            //var returnFileName = serverFileName;
            var toFileFullPath = Utilities.GetMapPath(dirPath);
            if (!Directory.Exists(toFileFullPath))
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            postedFile.SaveAs(toFileFullPath + newFileName);


            //检查图片尺寸是否超出限制
            if (_siteConfig.AttachImgMaxHeight > 0 || _siteConfig.AttachImgMaxWidth > 0)
            {
                Thumbnail.MakeThumbnailImage(toFileFullPath + newFileName, toFileFullPath + newFileName, _siteConfig.AttachImgMaxWidth, _siteConfig.AttachImgMaxHeight);
            }

            newPhoto.PhotoId = Guid.NewGuid();
            newPhoto.PhotoPath = serverFileName;

            //是否生成缩略图
            if (isThumbnail && _siteConfig.ThumbnailWidth > 0 && _siteConfig.ThumbnailHeight > 0)
            {
                Thumbnail.MakeThumbnailImage(toFileFullPath + newFileName, toFileFullPath + "thumb_" + newFileName, _siteConfig.ThumbnailWidth, _siteConfig.ThumbnailHeight, "Cut");
                //returnFileName += "," + serverThumbnailFileName; //返回缩略图，以逗号分隔开
                newPhoto.ThumbPath = serverThumbnailFileName;
            }
            //是否打图片水印
            if (IsWaterMark(fileType) && isWater)
            {
                switch (_siteConfig.WatermarkType)
                {
                    case 1:
                        WaterMark.AddImageSignText(serverFileName, serverFileName,
                            _siteConfig.WatermarkText, _siteConfig.WatermarkPosition,
                            _siteConfig.WatermarkImgQuality, _siteConfig.WatermarkFont, _siteConfig.WatermarkFontSize);
                        break;
                    case 2:
                        WaterMark.AddImageSignPic(serverFileName, serverFileName,
                            _siteConfig.WatermarkPic, _siteConfig.WatermarkPosition,
                            _siteConfig.WatermarkImgQuality, _siteConfig.WatermarkTransparency);
                        break;
                }
            }

            return newPhoto;

        }

        /// <summary>
        /// 从物理路径上删除文件
        /// </summary>
        /// <param name="relativePath">文件的相对路径</param>
        /// <returns>返回是否删除成功/是否存在该文件</returns>
        public bool DeleteFileInPhysical(string relativePath)
        {
            if (String.IsNullOrEmpty(relativePath))
                return false;
            var fileFullPath = Utilities.GetMapPath(relativePath);
            if (!File.Exists(fileFullPath)) return false;
                File.Delete(fileFullPath);
            return true;
        }

        //public string FileSaveAs(HttpPostedFile postedFile, bool isThumbnail, bool isWater, bool isImage)
        //{
        //    return FileSaveAs(postedFile, isThumbnail, isWater, isImage, false);
        //}

        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否添加水印</param>
        /// <param name="isImage"></param>
        /// <param name="isReOriginal">是否使用原始名字存储文件</param>
        /// <returns>上传后的文件信息</returns>
        public string FileSaveAs(HttpPostedFile postedFile, bool isThumbnail, bool isWater, bool isImage = false, bool isReOriginal = false)
        {
            var fileType = Utilities.GetFileTypeName(postedFile.FileName);
            var fileSize = postedFile.ContentLength;
            var fileName =
                postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
            var newFileName = Utilities.GetRamCodeOnDate() + "." + fileType;
            if (isReOriginal)
            {
                //如果 isReOriginal为True，那就使用原始名字
                newFileName = fileName;
            }
            if (!CheckFileExt(fileType))
            {
                return "{\"msg\": \"0\", \"msgbox\": \"不允许上传" + fileType + "类型的文件！\"}";
            }
            if (isImage && !IsImage(fileType))
            {
                return "{\"msg\": \"0\", \"msgbox\": \"对不起，仅允许上传图片文件！\"}";
            }
            if (!CheckFileSize(fileSize))
            {
                return "{\"msg\": \"0\", \"msgbox\": \"上传内容大小超出限制\"}";
            }

            var dirPath = GetUpLoadPath();

            //存储至物理路径
            var toFileFullPath = Utilities.GetMapPath(dirPath);
            if (!Directory.Exists(toFileFullPath))
            {
                Directory.CreateDirectory(toFileFullPath);
            }
            postedFile.SaveAs(toFileFullPath + newFileName);

            var serverFileName = dirPath + newFileName;
            var serverThumbnailFileName = dirPath + "thumb_" + newFileName;

            var returnFileName = serverFileName;

            //如果是图片，检查图片尺寸是否超出限制
            if (IsImage(fileType) && (_siteConfig.AttachImgMaxHeight > 0 || _siteConfig.AttachImgMaxWidth > 0))
            {
                Thumbnail.MakeThumbnailImage(toFileFullPath + newFileName, toFileFullPath + newFileName, _siteConfig.AttachImgMaxWidth, _siteConfig.AttachImgMaxHeight);
            }
            //是否生成缩略图
            if (IsImage(fileType) && isThumbnail && _siteConfig.ThumbnailWidth > 0 && _siteConfig.ThumbnailHeight > 0)
            {
                //参数“cut"表示切掉而不按照比例裁剪
                Thumbnail.MakeThumbnailImage(toFileFullPath + newFileName, toFileFullPath + "thumb_" + newFileName, _siteConfig.ThumbnailWidth, _siteConfig.ThumbnailHeight, "Cut");
                //增加缩略图相对路径，以逗号分隔开
                returnFileName += "," + serverThumbnailFileName;
            }
            //是否打图片水印
            if (IsWaterMark(fileType) && isWater)
            {
                switch (_siteConfig.WatermarkType)
                {
                    case 1:
                        WaterMark.AddImageSignText(serverFileName, serverFileName,
                            _siteConfig.WatermarkText, _siteConfig.WatermarkPosition,
                            _siteConfig.WatermarkImgQuality, _siteConfig.WatermarkFont, _siteConfig.WatermarkFontSize);
                        break;
                    case 2:
                        WaterMark.AddImageSignPic(serverFileName, serverFileName,
                            _siteConfig.WatermarkPic, _siteConfig.WatermarkPosition,
                            _siteConfig.WatermarkImgQuality, _siteConfig.WatermarkTransparency);
                        break;
                }
            }
            //如果需要返回原文件名
            if (isReOriginal)
            {
                return "{\"msg\": \"1\", \"msgbox\": \"" + returnFileName + "\", mstitle: \"" + fileName + "\"}";
            }
            return "{\"msg\": \"1\", \"msgbox\": \"" + returnFileName + "\"}";
        }

        #region 私有方法
        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <returns>相对路径</returns>
        private string GetUpLoadPath()
        {
            var path = String.Empty; //站点目录+上传目录
            switch (_siteConfig.AttachSave)
            {
                case 1: //按年月日每天一个文件夹
                    path = _siteConfig.WebPath + _siteConfig.AttachPath + "/" + DateTime.Now.ToString("yyyyMMdd");
                    break;
                case 2: //按年月/日存入不同的文件夹
                    path = _siteConfig.WebPath + _siteConfig.AttachPath + "/" + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                    break;
                case 3: //按年月存入不同的文件夹
                    path = _siteConfig.WebPath + _siteConfig.AttachPath + "/" + DateTime.Now.ToString("yyyyMM");
                    break;
                default: //上传的内容直接放在当前的文件夹中 
                    path = _siteConfig.WebPath + _siteConfig.AttachPath;
                    break;
            }
            return path + "/";
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string fileExt)
        {
            //判断是否开启水印
            if (_siteConfig.WatermarkType <= 0) return false;
            //判断是否可以打水印的图片类型
            var markValidateTypeList = new ArrayList {"bmp", "jpeg", "jpg", "png"};
            return markValidateTypeList.Contains(fileExt.ToLower());
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        private static bool IsImage(string fileExt)
        {
            var imageTypeList = new ArrayList {"bmp", "jpeg", "jpg", "gif", "png"};
            return imageTypeList.Contains(fileExt.ToLower());
        }

        /// <summary>
        /// 检查文件类型是否合法
        /// 危险文件类型：.asp .aspx .php .jsp .htm .html
        /// 合法文件类型：请查看定义的CustomSiteConfig
        /// </summary>
        /// <param name="fileExt">文件类型名</param>
        /// <returns>布尔类型</returns>
    
        private bool CheckFileExt(string fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "php", "jsp", "htm", "html" };
            if (excExt.Any(t => String.Equals(t, fileExt, StringComparison.CurrentCultureIgnoreCase)))
            {
                return false;
            }
            //检查合法文件
            var allowExt = _siteConfig.AttachExtension.Split(',');
            return allowExt.Any(t => String.Equals(t, fileExt, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="fileSize">文件大小(KB)</param>
        private bool CheckFileSize(int fileSize)
        {
            return _siteConfig.AttachImgSize <= 0 || fileSize <= _siteConfig.AttachImgSize * 1024;
        }

        #endregion
    }
}