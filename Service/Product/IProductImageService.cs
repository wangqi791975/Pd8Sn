using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 商品图片服务
    /// </summary>
    public interface IProductImageService
    {
        /// <summary>
        /// 根据图片名称获取完整的图片URL路径
        /// </summary>
        /// <param name="photoName">图片名称</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>返回图片的URL路径，eg:http://xxx.panduo.com/photo1/8/A/034D2678-1E61-4857-B5E6-A026917061A8_240x120.jpg
        /// </returns>
        string GetProductImageUrl(string photoName, int width, int height);

        /// <summary>
        /// 根据图片名称获取完整的图片URL路径
        /// </summary>
        /// <param name="photoName">图片名称</param>
        /// <param name="size">图片尺寸，正方形图片</param>
        /// <returns>返回图片的URL路径，eg:http://xxx.panduo.com/photo1/8/A/034D2678-1E61-4857-B5E6-A026917061A8_240x240.jpg
        /// </returns>
        string GetProductImageUrl(string photoName,int size);

        /// <summary>
        /// 根据图片名称获取完整的图片URL路径
        /// </summary>
        /// <param name="photoName">图片名称</param> 
        /// <returns>返回图片的URL路径，eg:http://xxx.panduo.com/photo1/8/A/034D2678-1E61-4857-B5E6-A026917061A8.jpg
        /// </returns>
        string GetProductImageUrl(string photoName);

        /// <summary>
        /// 获取NoPhoto完整的图片URL路径
        /// </summary>
        /// <param name="size">图片尺寸，正方形图片</param>
        /// <returns></returns>
        string GetNoPhotoImageUrl(int size); 

        /// <summary>
        /// 获取NoPhoto完整的图片URL路径
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        string GetNoPhotoImageUrl(int width, int height); 
    }
}
