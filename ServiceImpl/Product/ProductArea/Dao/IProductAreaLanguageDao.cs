using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.ProductArea.Dao
{
    public interface IProductAreaLanguageDao : IBaseDao<ProductAreaDescPo, int>
    {
        /// <summary>
        /// 判断该产品专区对应语种名称是否存在
        /// </summary>
        /// <param name="productAreaId"></param>
        /// <param name="name"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        bool IsAreaLanguageExistd(int productAreaId, int languageId, string name);

        /// <summary>
        /// 获取产品专区Id和语种 获取产品多语种名称
        /// </summary>
        /// <param name="productAreaId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ProductAreaDescPo GetAreaLanguage(int productAreaId, int languageId);


        /// <summary>
        /// 获取专区多语言列表
        /// </summary>
        /// <param name="productAreaId">专区Id</param>
        /// <returns></returns>
        IList<ProductAreaDescPo> GetAllProductAreaLanguages(int productAreaId);
    }
}
