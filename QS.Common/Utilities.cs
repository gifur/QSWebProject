using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace QS.Common
{
    public class Utilities
    {
        #region 文件操作

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="filepath">原图路径</param>
        public static void DeleteUpFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return;
            }
            var thumbnailpath = filepath.Substring(0, filepath.LastIndexOf("/", StringComparison.Ordinal)) + "thumb_" + filepath.Substring(filepath.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var thumbFullPath = GetMapPath(thumbnailpath); //宿略图
            var fullpath = GetMapPath(filepath); //原图
            if (File.Exists(fullpath)) 
            {
                File.Delete(fullpath);
            }
            if (File.Exists(thumbFullPath))
            {
                File.Delete(thumbFullPath);
            }
        }

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>不含点操作符的文件类型名</returns>
        public static string GetFileTypeName(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                return "";
            }
            return filePath.LastIndexOf(".", StringComparison.Ordinal) > 0 ? filePath.Substring(filePath.LastIndexOf(".", StringComparison.Ordinal) + 1) : "";
        }

        public static string GetFileNameWithoutType(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                return "";
            var index = filePath.LastIndexOf("/", StringComparison.Ordinal);
            var dotIndex = filePath.LastIndexOf(".", StringComparison.Ordinal);

            if (index >= 0)
            {
                return dotIndex >= 0 ? filePath.Substring(index + 1, dotIndex - index - 1) : filePath.Substring(index + 1);
            }
            return dotIndex >= 0 ? filePath.Substring(0, dotIndex) : filePath;
        }

        /// <summary>
        /// 返回相对路径的物理路径，如果是网络路径则直接返回
        /// </summary>
        /// <param name="strPath">相对文件路径</param>
        /// <returns></returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }

            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        #endregion

        #region 根据日期生成唯一的字符串码
        /// <summary>
        /// 根据日期生成唯一字符串码
        /// </summary>
        /// <returns>生成的时间字符串</returns>
        public static string GetRamCodeOnDate()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        #endregion

        #region 根据日期生成的GUID,用于数据库中表的主键生成

        /// <summary>
        /// 返回 GUID 用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB (GUID 与时间混合型) 类型 GUID 数据</returns>
        public static Guid NewComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        public static DateTime GetDateFromComb(Guid guid)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = guid.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays.
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints 
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);

            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        #endregion

        #region 截取字符长度
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHtml(inputString);
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var tempString = "";
            var s = ascii.GetBytes(inputString);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            var mybyte = Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        #region 清除HTML标记
        public static string DropHtml(string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString)) return "";
            //删除脚本  
            htmlString = Regex.Replace(htmlString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            htmlString = Regex.Replace(htmlString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"-->", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            htmlString = Regex.Replace(htmlString, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            //htmlString = htmlString.Replace("<", "");
            //htmlString = htmlString.Replace(">", "");
            //htmlString = htmlString.Replace("\r\n", "");
            htmlString = HttpContext.Current.Server.HtmlEncode(htmlString).Trim();
            return htmlString;
        }
        #endregion

        #region 清除HTML标记且返回相应的长度
        public static string DropHtml(string htmlString, int strLen)
        {
            return CutString(DropHtml(htmlString), strLen);
        }
        #endregion

        #region 获取html中的图片地址

        ///   <summary>
        ///   取出文本中的图片地址
        ///   </summary>
        ///   <param   name="htmlStr">HTMLStr</param>
        public static string GetImgUrl(string htmlStr)
        {
            var str = string.Empty;
            var r = new Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>", RegexOptions.Compiled);
            var m = r.Match(htmlStr.ToLower());
            if (!m.Success) return str;
            str = m.Result("${url}");
            var regex = new Regex("\"[^\"]*\"");
            var result = regex.Match(str).Value.Replace("\"", "");
            return result;
        }

        #endregion

        /// <summary>
        /// 暂未使用 取得HTML中所有图片的 URL。 
        /// fetch from : http://it.chinawin.net/softwaredev/article-1094f.html
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签
            var regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串
            var matches = regImg.Matches(sHtmlText);

            var i = 0;
            var sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;

            return sUrlList;
        }

        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Htmls(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string ihtml = input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region MD5加密
        public static string MD5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var data = System.Text.Encoding.Default.GetBytes(pwd);
            var md5Data = md5.ComputeHash(data);
            md5.Clear();
            return md5Data.Aggregate("", (current, t) => current + t.ToString("x").PadLeft(2, '0'));
        }
        #endregion
    }
}
