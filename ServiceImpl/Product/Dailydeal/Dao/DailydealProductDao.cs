//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：DailydealProductDao.cs
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
using Com.Panduo.Entity.Product.Dailydeal;

namespace Com.Panduo.ServiceImpl.Product.Dailydeal.Dao
{
    public class DailydealProductDao : BaseDao<DailydealProductPo, int>, IDailydealProductDao
    {
        public IList<DailydealProductPo> GetDailydealProductsByProductIds(string productIds)
        {
            if (productIds.IsNullOrEmpty())
            {
                return new List<DailydealProductPo>();
            }
            return FindDataByHql(string.Format("from DailydealProductPo where ProductId in ({0})", productIds));
        }

        public DailydealProductPo GetDailydealProduct(int productId)
        {
            return GetOneObject("from DailydealProductPo where ProductId=?", productId);
        }

        public bool IsProductDailyDeal(int productId)
        {
            return !GetOneObject("from DailydealProductPo where ProductId=?", productId).IsNullOrEmpty();
        }
    }
}
