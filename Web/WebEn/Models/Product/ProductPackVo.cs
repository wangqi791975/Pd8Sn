//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ProductPackVo.cs
//创 建 人：罗海明
//创建时间：2015/01/16 09:49:50 
//功能说明：产品大小包装VO类
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//----------------------------------------------------------------- 
using System.Collections.Generic;

namespace Com.Panduo.Web.Models.Product
{
    public class ProductPackVo
    {
        /// <summary>
        /// 主产品信息
        /// </summary>
        public Service.Product.ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 大小包装产品信息
        /// </summary>
        public IList<Service.Product.ProductInfo> OtherPack { get; set; }
    }
}