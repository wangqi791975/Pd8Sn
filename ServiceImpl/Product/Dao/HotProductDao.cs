//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：HotProductDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 14:40:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{ 
    public class HotProductDao:BaseDao<HotProductPo,int>, IHotProductDao
    {
        public HotProductPo GetHotProductByProductId(int productId)
        {
            return GetOneObject("from HotProductPo where ProductId = ?", productId);
        }

        public IList<HotProductPo> GetHotProductsByProductIds(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return  new List<HotProductPo>();
            }

            return FindDataByHql(string.Format("from HotProductPo where ProductId in ({0})", productIds));
        }
    } 
}
   