using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{
    public class ProductSimilarIgnoreViewDao : BaseDao<ProductSimilarIgnoreViewPo, int>, IProductSimilarIgnoreViewDao
    { 
        public IList<ProductSimilarIgnoreViewPo> GetProductSimilarIgnoreViewPos(int productId)
        {
            return FindDataByHql("from ProductSimilarIgnoreViewPo where ProductId = ?", productId);
        }
    }
}
