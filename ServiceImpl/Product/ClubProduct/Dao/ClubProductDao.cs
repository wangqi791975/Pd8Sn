//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：ClubProductDao.cs
//创 建 人：罗海明
//创建时间：2015/03/25 10:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 添加获取方法
//修改人：   罗海明
//修改时间： 2015/03/25 11:03:50 
//修改内容： 
//-----------------------------------------------------------------  
using System;
using System.Collections.Generic;
using Com.Panduo.Common;
using Com.Panduo.Entity.Product.Club;

namespace Com.Panduo.ServiceImpl.Product.ClubProduct.Dao
{
    public class ClubProductDao : BaseDao<ClubProductPo, int>, IClubProductDao
    {
        public ClubProductPo GetClubProduct(int productId)
        {
            return GetOneObject("from ClubProductPo where ProductId = ?", productId);
        }

        public ClubProductPo GetVaildClubProductByProductId(int productId)
        {
            return GetOneObject("from ClubProductPo where ProductId = ? and DateCreated<=? and DateEnded>=?", new object[] { productId, DateTime.Now.Date, DateTime.Now.Date });
        }

        public IList<ClubProductPo> GetVaildClubProductByProductIds(IList<int> productIds)
        {
            if (!productIds.IsNullOrEmpty())
            {
                var obj = new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("id", productIds),
                        new KeyValuePair<string, object>("DateCreated", DateTime.Now.Date),
                        new KeyValuePair<string, object>("DateEnded", DateTime.Now.Date),
                    };
                return FindDataByHql("from ClubProductPo  WHERE  ProductId in (:id) and DateCreated<=:DateCreated and DateEnded>=:DateEnded", obj);
            }
            return new List<ClubProductPo>();
        }

        public void DeleteClubProduct(int productId)
        {
            DeleteObjectByHql("DELETE FROM ClubProductPo WHERE ProductId = ?", productId);
        }
    }
}