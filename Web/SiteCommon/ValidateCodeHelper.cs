using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 验证码生成辅助类
    /// </summary>
    public class ValidateCodeHelper
    {  
        //颜色列表，用于验证码、噪线、噪点
        private static readonly Color[] Colors = { Color.Teal, Color.Crimson, Color.Blue, Color.SaddleBrown, Color.DarkGreen, Color.Navy, Color.DeepPink, Color.Purple };

        //字体列表，用于验证码
        private static readonly string[] Fonts = { "Times New Roman", "Arial" };

        //验证码的字符集，去掉了一些容易混淆的字符
        private static readonly Char[] Chars = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// 验证码字符串
        /// </summary>
        public string ValidateCode { private set; get; }

        /// <summary>
        /// 生成指定规格的验证码(4位验证码,宽度70px,高度25px,jpeg格式)
        /// </summary>
        /// <returns></returns>
        public byte[] CreateValidateImage()
        {
            return CreateValidateImage(4, 70, 25);
        }

        /// <summary>
        /// 生成指定规格的验证码(宽度70px,高度25px,jpeg格式)
        /// </summary>
        /// <param name="length">验证码字符个数</param> 
        /// <returns>验证码图片字节数据</returns>
        public byte[] CreateValidateImage(int length)
        {
            return CreateValidateImage(length, 70, 25);
        }

        /// <summary>
        /// 生成指定规格的验证码(默认生成jpeg格式)
        /// </summary>
        /// <param name="length">验证码字符个数</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <returns>验证码图片字节数据</returns>
        public byte[] CreateValidateImage(int length,int imageWidth,int imageHeight)
        {
           return CreateValidateImage(length,imageWidth,imageHeight,ImageFormat.Jpeg);
        }

        /// <summary>
        /// 生成指定规格的验证码
        /// </summary>
        /// <param name="length">验证码字符个数</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="imageFormat">图片格式</param>
        /// <returns>验证码图片字节数据</returns>
        public byte[] CreateValidateImage(int length,int imageWidth,int imageHeight,ImageFormat imageFormat)
        {
            var validateCodes = new List<char>(length);
            var random = new Random();

            //随机生成验证码字符串(不重复) 
            while (validateCodes.Count < length)
            {
                var codeChar = Chars[random.Next(Chars.Length)];
                if (!validateCodes.Contains(codeChar))
                {
                    validateCodes.Add(codeChar);
                }
            }

            var bitmap = new Bitmap(imageWidth, imageHeight);
            var g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);

            //画噪线
            for (var i = 0; i < length; i++)
            {
                var x1 = random.Next(bitmap.Width);
                var y1 = random.Next(bitmap.Height);
                var x2 = random.Next(bitmap.Width);
                var y2 = random.Next(bitmap.Height);
                var color = Colors[random.Next(Colors.Length)];
                g.DrawLine(new Pen(color), x1, y1, x2, y2);
            }

            //画验证码字符串
            var widthAvg = Convert.ToSingle(decimal.Divide(bitmap.Width, length) - 3);
            var heightStart = Convert.ToSingle(bitmap.Height<30?2:(decimal.Divide(bitmap.Height, 2) - 10));

            for (var i = 0; i < validateCodes.Count; i++)
            {
                var fontName = Fonts[random.Next(Fonts.Length)];
                var font = new Font(fontName, 16);
                var color = Colors[random.Next(Colors.Length)];
                g.DrawString(validateCodes[i].ToString(), font, new SolidBrush(color), i * widthAvg + 4, heightStart);
            }

            //画噪点
            for (var i = 0; i < 100; i++)
            {
                var x = random.Next(bitmap.Width);
                var y = random.Next(bitmap.Height);
                var clr = Colors[random.Next(Colors.Length)];
                bitmap.SetPixel(x, y, clr);
            }

            //边框
            g.DrawRectangle(new Pen(Color.Gray), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

            //保存验证码字符串
            ValidateCode = new string(validateCodes.ToArray());

            //保存图片数据
            var stream = new MemoryStream();
            bitmap.Save(stream, imageFormat);

            //输出图片流 
            return stream.ToArray();
        }

    }
}