//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductVo.cs
//创 建 人：罗海明
//创建时间：2015/01/05 09:49:50 
//功能说明：产品展示VO类
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//----------------------------------------------------------------- 
using System;
using System.Collections.Generic;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;

namespace Com.Panduo.Web.Models.Product
{
     [Serializable]
    public class ProductVo
    {
         /// <summary>
         ///类别信息
         /// </summary>
        public Category Category { get; set; }

         /// <summary>
         /// 类别广告信息 
         /// </summary>
        public CategoryAdvertisement CategoryAdvertisement { get; set; }

         /// <summary>
         /// 类别多语种描述
         /// </summary>
        public CategoryLanguage CategoryLanguage { get; set; }

        /// <summary>
        /// 产品分页数据
        /// </summary>
        public PageData<Service.Product.ProductInfo> ProductInfo { get; set; }

         /// <summary>
         /// HotItems
         /// </summary>
        public IList<Service.Product.ProductInfo> HotItems { get; set; }

         /// <summary>
         /// 判断是否一级类别
         /// </summary>
        public bool IsRoot { get; set; }

         /// <summary>
         /// 是否末级类别
         /// </summary>
        public bool IsLeaf { get; set; }

    }
}