using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.ProductArea.Dao
{
    public class ProductAreaLanguageDao : BaseDao<ProductAreaDescPo, int>, IProductAreaLanguageDao
    {

        public IList<ProductAreaDescPo> GetAllProductAreaLanguages(int productAreaId)
        {
            return FindDataByHql("from ProductAreaDescPo where ProductAreaId=?", productAreaId);
        }

        public bool IsAreaLanguageExistd(int productAreaId, int languageId, string name)
        {
            return GetOneObject("from ProductAreaDescPo where ProductAreaId=? and LanguageId=? and Name=?", new object[] { productAreaId, languageId, name }) != null;
        }

        public ProductAreaDescPo GetAreaLanguage(int productAreaId, int languageId)
        {
            return GetOneObject("from ProductAreaDescPo where ProductAreaId=? and LanguageId=?", new object[] { productAreaId, languageId });
        }
    }
}
