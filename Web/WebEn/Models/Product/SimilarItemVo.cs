//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：SimilarItemVo.cs
//创 建 人：罗海明
//创建时间：2015/01/15 14:40:40 
//功能说明：相似商品Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using Com.Panduo.Service;
using Com.Panduo.Service.Product.Category;

namespace Com.Panduo.Web.Models.Product
{
    [Serializable]
    public class SimilarItemVo
    {
        /// <summary>
        /// 类别多语种描述
        /// </summary>
        public CategoryLanguage CategoryLanguage { get; set; }

        /// <summary>
        /// 参照产品
        /// </summary>
        public Service.Product.ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 从属产品（匹配产品或相似产品）分页数据
        /// </summary>
        public PageData<Service.Product.ProductInfo> SimilarProductInfo { get; set; }

    }
}