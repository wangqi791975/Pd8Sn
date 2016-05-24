//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IProductDao.cs
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
using Com.Panduo.Service;
using Com.Panduo.Service.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public interface IProductDao:IBaseDao<ProductPo,int>
    {
        /// <summary>
        /// 根据产品ID判断产品是否存在
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns></returns>
        bool IsExists(int productId);

        bool IsExistsByCode(string productCode);

        /// <summary>
        /// 根据产品编号获取产品信息
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns></returns>
        ProductPo GetProductByCode(string productCode);

        /// <summary>
        /// 直接查询数据库得到商品数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="searchDictionary">查询条件</param>
        /// <returns></returns>
        PageData<ProductPo> FindProductsForAdminList(int currentPage, int pageSize, IDictionary<ProductSearchCriteria, object> searchDictionary);

        /// <summary>
        /// 根据产品ID获取一批产品信息
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        IList<ProductPo> GetProductsByProductIds(string productIds);

    } 
}
   