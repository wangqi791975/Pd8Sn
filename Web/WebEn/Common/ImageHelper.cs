using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service;

namespace Com.Panduo.Web.Common
{
    /// <summary>
    /// 产品图片辅助类
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 根据图片编码取得图片的URL信息
        /// </summary>
        /// <param name="imageName">图片名称</param>
        /// <returns></returns>
        public static string GetImageUrl(string imageName)
        {
            return string.IsNullOrEmpty(imageName) ? string.Empty : ServiceFactory.ProductImageService.GetProductImageUrl(imageName);
        }

        /// <summary>
        /// 根据图片编码取得图片的URL信息, 并且图片大小按width和heigh进行缩放
        /// </summary>
        /// <param name="imageName">图片名称</param>
        /// <param name="size">正方形高或者宽</param> 
        /// <returns></returns>
        public static string GetImageUrl(string imageName, int size)
        { 
            return GetImageUrl(imageName, size, size); 
        }

        /// <summary>
        /// 根据图片编码取得图片的URL信息, 并且图片大小按width和heigh进行缩放
        /// </summary>
        /// <param name="imageName">图片名称</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static string GetImageUrl(string imageName, int width, int height)
        { 
            return string.IsNullOrEmpty(imageName) ? string.Empty : ServiceFactory.ProductImageService.GetProductImageUrl(imageName, width, height);
        }

        /// <summary>
        /// 获取类别图片
        /// </summary>
        /// <param name="imageName">类别图片名称</param>
        /// <returns></returns>
        public static string GetCategoryImageUrl(string imageName)
        {
            return string.Format("{0}{1}", ConfigHelper.CateogryImageHost, imageName);
        }

        /// <summary>
        /// 获取类别广告图片
        /// </summary>
        /// <param name="imageName">类别广告图片名称</param>
        /// <returns></returns>
        public static string GetCategoryAdImageUrl(string imageName)
        {
            return string.Format("{0}{1}", ConfigHelper.CateogryImageHost, imageName);
        }

        /// <summary>
        /// 根据Mail Logo相对路径获取完整URL信息
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string GetMailLogoImageUrl(string imagePath)
        {
            return string.Format("{0}{1}", ConfigHelper.MailLogoHost, imagePath);
        }
    }
}