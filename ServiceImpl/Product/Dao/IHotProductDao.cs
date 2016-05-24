//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IHotProductDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 13:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public interface IHotProductDao:IBaseDao<HotProductPo,int>
    {
        /// <summary>
        /// 根据产品Id获取热销产品信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>HotProductPo</returns>
        HotProductPo GetHotProductByProductId(int productId);

        /// <summary>
        /// 批量获取产品热销信息
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        IList<HotProductPo> GetHotProductsByProductIds(string productIds);
    } 
}
   