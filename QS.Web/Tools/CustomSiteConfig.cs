namespace QS.Web.Tools
{
    public enum CustomFileType
    {
        /// <summary>
        /// 图片类型
        /// </summary>
        Image = 0,
        /// <summary>
        /// 其他类型，文件
        /// </summary>
        File = 1,
        /// <summary>
        /// 视频
        /// </summary>
        Media = 2
    }
    public enum StoreAreaForUpload
    {
        /// <summary>
        /// 用户
        /// </summary>
        ForUser = 0,
        /// <summary>
        /// 图片分享
        /// </summary>
        ForGallery = 1,
        /// <summary>
        /// 视频分享
        /// </summary>
        ForVideo = 2,
        /// <summary>
        /// 书籍推荐
        /// </summary>
        ForBook = 3,
        /// <summary>
        /// 心理文章
        /// </summary>
        ForArticle = 4,
        /// <summary>
        /// 新闻速递
        /// </summary>
        ForNews = 5,
        /// <summary>
        /// 心理反馈
        /// </summary>
        ForFeedback
    };

    public enum WatermarkEnum
    {
        Words = 0,
        Picture = 1
    }

    /// <summary>
    /// 预先设定的一些格式的信息
    /// </summary>
    public class CustomSiteConfig
    {
        /// <summary>
        /// 允许上传的非图片文件的大小
        /// </summary>
        public int AttachFileSize { get; set; }
        /// <summary>
        /// 允许上传的文件类型的大小
        /// </summary>
        public int AttachImgSize { get; set; }
        /// <summary>
        /// 图片最大高度
        /// </summary>
        public int AttachImgMaxHeight { get; set; }
        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public int AttachImgMaxWidth { get; set; }
        /// <summary>
        /// 缩略图宽度
        /// </summary>
        public int ThumbnailWidth { get; set; }
        /// <summary>
        /// 缩略图高度
        /// </summary>
        public int ThumbnailHeight { get; set; }
        /// <summary>
        /// 水印类型1表示文字 2表示图片
        /// </summary>
        public int WatermarkType { get; set; }
        /// <summary>
        /// 水印所显示的文字
        /// </summary>
        public string WatermarkText { get; set; }
        /// <summary>
        /// 水印质量
        /// </summary>
        public int WatermarkImgQuality { get; set; }
        /// <summary>
        /// 水印图片名称或者说路径
        /// </summary>
        public string WatermarkPic { get; set; }
        /// <summary>
        /// Web目录，默认设为"/"
        /// </summary>
        public string WebPath { get; set; }
        /// <summary>
        /// 上传所在的文件夹
        /// </summary>
        public string AttachPath { get; set; }
        /// <summary>
        /// 水印位置 图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下
        /// </summary>
        public int WatermarkPosition { get; set; }
        /// <summary>
        /// 水印字体
        /// </summary>
        public string WatermarkFont { get; set; }
        /// <summary>
        /// 水印字体大小
        /// </summary>
        public int WatermarkFontSize { get; set; }
        /// <summary>
        /// 水印的透明度 1--10 10为不透明
        /// </summary>
        public int WatermarkTransparency { get; set; }
        /// <summary>
        /// 文件存放的方式，有四种 yyyyMMdd 或 yyyyMM/dd 或 yyyyMM 或 直接存储在当前路径的目录下
        /// </summary>
        public int AttachSave { get; set; }
        /// <summary>
        /// 允许上传或使用的文件扩展名
        /// </summary>
        public string AttachExtension { get; set; }
        /// <summary>
        /// 生成缩略图的方式，有三种：具体请看Thumbnail类，"HW" "W" "H" "Cut"
        /// </summary>
        public string ThumbFormationType { get; set; }

        /// <summary>
        /// 初始化CustomSiteConfig类，默认上传的是图片类型
        /// </summary>
        /// <param name="fileType"></param>
        public CustomSiteConfig(CustomFileType fileType)
        {
            WebPath = "/";
            
            if (fileType == CustomFileType.Image)
            {

                AttachPath = "Images";
                AttachSave = 1;
                AttachExtension = "gif,jpg,png,bmp";
                AttachFileSize = 51200;

                /*图片格式*/
                AttachImgSize = 10240;
                AttachImgMaxHeight = 800;
                AttachImgMaxWidth = 800;
                //缩略图所需图片大小
                ThumbnailHeight = 300;
                ThumbnailWidth = 300;
                ThumbFormationType = "Cut";
                //以下为水印所需
                WatermarkType = 1; //1为文字，2为图片
                WatermarkImgQuality = 80;
                WatermarkPic = "/Content/qs_watermark.png";
                WatermarkText = "求索工作室";
                WatermarkPosition = 9;
                WatermarkFont = "Tahoma";
                WatermarkFontSize = 12;
                WatermarkTransparency = 5;
            }


            if (fileType == CustomFileType.Media)
            {
                AttachPath = "Attached";
                AttachSave = 1;
                AttachExtension = "gif,jpg,png,bmp,rar,zip,doc,xls,txt";
                AttachFileSize = 51200;
            }

            if (fileType != CustomFileType.File) return;

            AttachPath = "Profiles";
            AttachExtension = "rar,zip,doc";
            AttachFileSize = 51200;
        }

        private CustomSiteConfig(){}
    }
}