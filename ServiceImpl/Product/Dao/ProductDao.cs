


//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ProductDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
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
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;
using Com.Panduo.Service;
using Com.Panduo.Service.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{
    public class ProductDao : BaseDao<ProductPo, int>, IProductDao
    {
        public bool IsExists(int productId)
        {
            return GetOneObject("from ProductPo where ProductId=?", new object[] { productId }) != null;
        }

        public bool IsExistsByCode(string productCode)
        {
            return GetOneObject("from ProductPo where ProductModel=?", new object[] { productCode }) != null;
        }

        public ProductPo GetProductByCode(string productCode)
        {
            return GetOneObject("from ProductPo where ProductModel = ?", productCode);
        }

        public PageData<ProductPo> FindProductsForAdminList(int currentPage, int pageSize, IDictionary<ProductSearchCriteria, object> searchDictionary)
        {
            var hqlHelper = new HqlHelper("Select p from ProductPo p");
            if (!searchDictionary.IsNullOrEmpty())
            {
                foreach (var item in searchDictionary)
                {
                    switch (item.Key)
                    {
                        case ProductSearchCriteria.Keyword:
                            hqlHelper.AddWhere("p.ProductModel", HqlOperator.Like, "ProductModel", item.Value);
                            break;
                    }
                }
            }
            hqlHelper.AddSorter("p.ProductId", false);
            return FindPageDataByHql(currentPage, pageSize, hqlHelper.Hql, hqlHelper.ParamMap);
        }

        public IList<ProductPo> GetProductsByProductIds(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return new List<ProductPo>();
            }

            return FindDataByHql(string.Format("from ProductPo where ProductId in ({0})", productIds));
        }
    }
}
