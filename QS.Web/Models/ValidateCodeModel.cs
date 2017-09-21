using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    /// <summary>
    /// 登录使用的验证码
    /// </summary>
    public class ValidateCodeModel
    {
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }


        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                /*画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                */

                //开始尝试
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.FromName("#999")), 0, 0, image.Width - 1, image.Height - 1);
                int randAngle = 45; //随机转动角度 
                //验证码旋转，防止机器识别   
                char[] chars = validateCode.ToCharArray();//拆散字符串成单字符数组 
                //文字距中 
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                //定义颜色 
                Color[] c = { Color.Black, Color.Red, Color.Blue, Color.Green, 
                                Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
                //定义字体 
                string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
                for (int i = 0; i < chars.Length; i++)
                {
                    int cindex = random.Next(7);
                    int findex = random.Next(5);
                    Font f = new Font(fonts[findex], 12, FontStyle.Bold);//字体样式，其中参数2为字体大小 
                    Brush b = new SolidBrush(c[cindex]);
                    Point dot = new Point(8, 11);
                    float angle = random.Next(-randAngle, randAngle);//转动的度数 
                    g.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置 
                    g.RotateTransform(angle);
                    g.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                    g.RotateTransform(-angle);//转回去 
                    g.TranslateTransform(2, -dot.Y);//移动光标到指定位置 
                }
                //结束尝试

                /*画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                */

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}