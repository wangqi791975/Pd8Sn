using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{
    public interface IProductSimilarIgnoreViewDao : IBaseDao<ProductSimilarIgnoreViewPo,int>
    {
        IList<ProductSimilarIgnoreViewPo> GetProductSimilarIgnoreViewPos(int productId);
    }
}
