using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.ProductArea.Dao
{
    public class ProductAreaDao : BaseDao<ProductAreaPo, int>, IProductAreaDao
    {
        public ProductAreaPo GetProductAreaById(int productAreaId)
        {
            return GetOneObject("from ProductAreaPo where ProductAreaId = ?", productAreaId);
        }
    }
}
