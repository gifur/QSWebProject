using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;

namespace QS.Web.Models
{
    public class ValidateCode2Model
    {
        private Color backgroundColor = Color.White;
        private Color chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color[] drawColors = new Color[] { Color.FromArgb(0x6b, 0x42, 0x26), Color.FromArgb(0x4f, 0x2f, 0x4f), Color.FromArgb(50, 0x99, 0xcc), Color.FromArgb(0xcd, 0x7f, 50), Color.FromArgb(0x23, 0x23, 0x8e), Color.FromArgb(0x70, 0xdb, 0x93), Color.Red, Color.FromArgb(0xbc, 0x8f, 0x8e) };
        private bool fontTextRenderingHint;
        private int imageHeight = 30;
        private int padding = 1;
        private int validataCodeLength = 4;
        private int validataCodeSize = 0x10;
        private string validateCodeFont = "Arial";

        public byte[] CreateImage(out string validataCode)
        {
            Bitmap bitmap;
            string formatString = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            GetRandom(formatString, this.ValidataCodeLength, out validataCode);
            var stream = new MemoryStream();
            this.ImageBmp(out bitmap, validataCode);
            bitmap.Save(stream, ImageFormat.Png);
            bitmap.Dispose();
            bitmap = null;
            stream.Close();
            stream.Dispose();
            return stream.GetBuffer();
        }

        private void CreateImageBmp(ref Bitmap bitMap, string validateCode)
        {
            Graphics graphics = Graphics.FromImage(bitMap);
            Random random = new Random();
            if (this.fontTextRenderingHint)
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }
            else
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Font font = new Font(this.validateCodeFont, (float)this.validataCodeSize, FontStyle.Regular);
            int maxValue = Math.Max((this.ImageHeight - this.validataCodeSize) - 5, 0);
            for (int i = 0; i < this.validataCodeLength; i++)
            {
                Color color = this.DrawColors[random.Next(this.DrawColors.Length)];
                Brush brush = new SolidBrush(color);
                int[] numArray = new int[] { ((i * this.validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                Point point = new Point(numArray[0], numArray[1]);
                graphics.DrawString(validateCode[i].ToString(), font, brush, (PointF)point);
            }
            graphics.Dispose();
        }

        private void DisposeImageBmp(ref Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            new Random();
            Point[] pointArray = new Point[2];
            Random random = new Random();
            for (int i = 0; i < (this.validataCodeLength * 2); i++)
            {
                Pen pen = new Pen(this.DrawColors[random.Next(this.DrawColors.Length)], 1f);
                pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                graphics.DrawLine(pen, pointArray[0], pointArray[1]);
            }
            graphics.Dispose();
        }

        private static void GetRandom(string formatString, int len, out string codeString)
        {
            codeString = string.Empty;
            string[] strArray = formatString.Split(new char[] { ',' });
            var random = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(0x186a0) % strArray.Length;
                codeString = codeString + strArray[index].ToString();
            }
        }

        private void ImageBmp(out Bitmap bitMap, string validataCode)
        {
            var width = (int)((this.validataCodeLength * this.validataCodeSize) * 1.2);
            bitMap = new Bitmap(width, this.ImageHeight);
            this.DisposeImageBmp(ref bitMap);
            this.CreateImageBmp(ref bitMap, validataCode);
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }
            set
            {
                this.backgroundColor = value;
            }
        }

        public Color ChaosColor
        {
            get
            {
                return this.chaosColor;
            }
            set
            {
                this.chaosColor = value;
            }
        }

        public Color[] DrawColors
        {
            get
            {
                return this.drawColors;
            }
            set
            {
                this.drawColors = value;
            }
        }

        private bool FontTextRenderingHint
        {
            get
            {
                return this.fontTextRenderingHint;
            }
            set
            {
                this.fontTextRenderingHint = value;
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }
            set
            {
                this.imageHeight = value;
            }
        }

        public string Name
        {
            get
            {
                return "线条干扰(彩色)";
            }
        }

        public int Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        public int ValidataCodeLength
        {
            get
            {
                return this.validataCodeLength;
            }
            set
            {
                this.validataCodeLength = value;
            }
        }

        public int ValidataCodeSize
        {
            get
            {
                return this.validataCodeSize;
            }
            set
            {
                this.validataCodeSize = value;
            }
        }

        public string ValidateCodeFont
        {
            get
            {
                return this.validateCodeFont;
            }
            set
            {
                this.validateCodeFont = value;
            }
        }
    }
}