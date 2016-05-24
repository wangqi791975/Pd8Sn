//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IProductDescDao.cs
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
    public interface IProductDescDao:IBaseDao<ProductDescPo,int>
    {
        /// <summary>
        /// 批量获取一批产品的多语种信息
        /// </summary>
        /// <param name="productIds"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
  
        IList<ProductDescPo> GetProductDescriptionsByProductIdAndLangId(string productIds, int languageId);

        ProductDescPo GetProductDescription(int productId, int languageId);

        IList<ProductDescPo> GetProductDescriptions(int productId);
    } 
}
   