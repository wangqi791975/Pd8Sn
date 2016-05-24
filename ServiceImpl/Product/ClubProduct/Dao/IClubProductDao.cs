//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IClubProductDao.cs
//创 建 人：罗海明
//创建时间：2015/03/25 10:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 添加接口方法
//修改人：   罗海明
//修改时间： 2015/03/25 11:03:50 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Entity.Product.Club;

namespace Com.Panduo.ServiceImpl.Product.ClubProduct.Dao
{
    public interface IClubProductDao:IBaseDao<ClubProductPo,int>
    {
        /// <summary>
        /// 获取club会员产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns>club会员产品</returns>
        ClubProductPo GetClubProduct(int productId);

        /// <summary>
        /// 获取有效的Club产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        ClubProductPo GetVaildClubProductByProductId(int productId);

        /// <summary>
        ///  获取有效的Club产品列表
        /// </summary>
        /// <param name="productIds">产品Ids</param>
        /// <returns></returns>
        IList<ClubProductPo> GetVaildClubProductByProductIds(IList<int> productIds);

        /// <summary>
        /// 删除club会员产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        void DeleteClubProduct(int productId);
    }
}