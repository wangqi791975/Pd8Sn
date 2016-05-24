using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.ProductArea.Dao
{
    public interface IProductAreaDao : IBaseDao<ProductAreaPo, int>
    {
        /// <summary>
        /// 获取专区实体
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <returns></returns>
        ProductAreaPo GetProductAreaById(int productAreaId);
    }
}
