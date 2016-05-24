//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IWishListTypeDescDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Customer;

namespace Com.Panduo.ServiceImpl.Customer.Product.Dao
{ 
    public interface IWishListTypeDescDao:IBaseDao<WishListTypeDescPo,int>
    {
        /// <summary>
        /// 根据语种Id获取WishList类型的列表
        /// </summary>
        /// <param name="languageId">语种Id</param>
        /// <returns>WishListTypeDescPo列表</returns>
        IList<WishListTypeDescPo> GetWishListTypeDesc(int languageId);
    } 
}
   