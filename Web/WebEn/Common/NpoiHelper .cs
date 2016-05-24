using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// Npoi辅助类
    /// </summary>
    public class NpoiHelper
    {
        /// <summary>
        /// 像素转换为NPOI宽度比例
        /// </summary>
        private static readonly float Pixel2NpoiWidthScale = 15F;
        /// <summary>
        /// 像素转换为NPOI高度比例
        /// </summary>
        private static readonly float Pixel2NpoiHightScale = 36.5F; 

        /// <summary>
        /// 像素转换为NPOI的宽度
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static short Pixel2NpoiWidth(int width)
        {
            return (short)(width * Pixel2NpoiHightScale);
        }

        /// <summary>
        /// 像素转换为NPOI的高度
        /// </summary>
        /// <param name="hight"></param>
        /// <returns></returns>
        public static short Pixel2NpoiHeight(int hight)
        {
            return (short)(hight * Pixel2NpoiWidthScale);
        }

        /// <summary>
        /// NPOI转换为像素的宽度
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static int Npoi2PixelWidth(short width)
        {
            return (int)(width / Pixel2NpoiHightScale);
        }

        /// <summary>
        /// NPOI转换为像素的高度
        /// </summary>
        /// <param name="hight"></param>
        /// <returns></returns>
        public static int Npoi2PixelHeight(short hight)
        {
            return (int)(hight / Pixel2NpoiWidthScale);
        } 
    }
}
