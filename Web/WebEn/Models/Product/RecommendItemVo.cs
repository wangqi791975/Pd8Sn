//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：RecommendItemVo.cs
//创 建 人：罗海明
//创建时间：2015/01/15 13:40:40 
//功能说明：推荐商品Vo
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.Product
{
    public class RecommendItemVo
    {
        /// <summary>
        /// 商品信息扩展列表
        /// </summary>
        public IList<ProductInfo> ProductInfo { get; set; }

        /// <summary>
        /// 主产品Id，用于More的时候跳转页面参数
        /// </summary>
        public int? MainProductId { get; set; }
    }
}