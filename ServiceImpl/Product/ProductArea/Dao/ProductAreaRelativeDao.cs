using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.ProductArea.Dao
{
    public class ProductAreaRelativeDao : BaseDao<ProductAreaRelativePo, int>, IProductAreaRelativeDao
    {
        /// <summary>
        /// 获取产品专信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        public ProductAreaRelativePo GetProductAreaRelative(int productId)
        {
            return GetOneObject("from ProductAreaRelativePo where ProductId=?", productId);
        }


        public ProductAreaRelativePo GetProductAreaRelative(int areaId, int productId)
        {
            return GetOneObject("from ProductAreaRelativePo where ProductAreaId=? and ProductId=?", new object[] { areaId, productId });
        }


        public IList<ProductAreaRelativePo> GetAllProductAreaRelativesByAreaId(int areaId)
        {
            return FindDataByHql("from ProductAreaRelativePo where ProductAreaId=?", areaId);
        }

        public void DeleteProductAreaRelative(int areaId)
        {
            DeleteObjectByHql("DELETE FROM ProductAreaRelativePo WHERE ProductAreaId = ?", areaId);
        }
    }
}
