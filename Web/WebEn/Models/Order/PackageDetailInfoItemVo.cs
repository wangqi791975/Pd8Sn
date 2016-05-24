//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：PackageDetailInfoItemVo.cs
//创 建 人：罗海明
//创建时间：2015/03/10 13:59:50 
//功能说明：包裹明细信息扩展类
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Service.Order;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Web.Models.Order
{
    public class PackageDetailInfoItemVo
    {
        /// <summary>
        /// 产品扩展信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

        /// <summary>
        /// 包裹明细信息
        /// </summary>
        public PackageDetail PackageDetail { get; set; }

        /// <summary>
        /// 包裹列表
        /// </summary>
        public IList<Package> PackageList { get; set; }
    }
}