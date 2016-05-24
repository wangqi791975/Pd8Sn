using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Product; 
using PhotosRule;

namespace Com.Panduo.ServiceImpl.Product
{
    /// <summary>
    /// 产品图片服务实现层
    /// </summary>
    public class ProductImageService : IProductImageService
    {  
        public string GetProductImageUrl(string photoName, int width, int height)
        {
            return PhotoRuleHelper.GetPhotoUrl(photoName, width, height);
        }

        public string GetProductImageUrl(string photoName, int size)
        {
            return GetProductImageUrl(photoName, size, size);
        }

        public string GetProductImageUrl(string photoName)
        {
            return PhotoRuleHelper.GetOriginalImageFullUrl(photoName);
        }

        public string GetNoPhotoImageUrl(int size)
        {
            return GetNoPhotoImageUrl(size, size);
        }

        public string GetNoPhotoImageUrl(int width, int height)
        {
            return PhotoRuleHelper.GetNoPhotoImageFullUrl(width, height);
        } 
    }
}
