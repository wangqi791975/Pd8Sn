using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.ProductArea.Dao
{
    public interface IProductAreaRelativeDao : IBaseDao<ProductAreaRelativePo, int>
    {
        /// <summary>
        /// 获取产品专信息
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ProductAreaRelativePo GetProductAreaRelative(int productId);
        /// <summary>
        /// 获取产品专信息
        /// </summary>
        /// <param name="areaId">专区Id</param>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ProductAreaRelativePo GetProductAreaRelative(int areaId, int productId);

        /// <summary>
        /// 获取专区下所有产品
        /// </summary>
        /// <param name="areaId">专区Id</param>
        /// <returns></returns>
        IList<ProductAreaRelativePo> GetAllProductAreaRelativesByAreaId(int areaId);

        /// <summary>
        /// 通过专区Id删除产品
        /// </summary>
        /// <param name="areaId">专区Id</param>
        void DeleteProductAreaRelative(int areaId);
    }
}
