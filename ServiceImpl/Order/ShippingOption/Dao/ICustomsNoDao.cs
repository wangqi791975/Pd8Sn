//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ICustomsNoDao.cs
//创 建 人：万天文
//创建时间：2015/03/02 16:43:01
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
using Com.Panduo.Entity.Shipping;
using Com.Panduo.Entity.ShoppingCart;

namespace Com.Panduo.ServiceImpl.Order.ShippingOption.Dao
{
    public interface ICustomsNoDao : IBaseDao<CustomsNoPo, int>
    {
        /// <summary>
        /// 得到关税信息
        /// </summary>
        /// <param name="shippingId">配送方式ID</param>
        /// <param name="countryId">国家ID</param>
        /// <returns>CustomsNoPo对象</returns>
        CustomsNoPo GetCustomsNoPo(int shippingId, int countryId);
    }
}
