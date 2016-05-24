//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：CustomsNoDao.cs     
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
    public class CustomsNoDao : BaseDao<CustomsNoPo, int>, ICustomsNoDao
    {
        public CustomsNoPo GetCustomsNoPo(int shippingId, int countryId)
        {
            return GetOneObject("from CustomsNoPo where ShippingId = ? and CountryId = ?", new object[] { shippingId, countryId }); ;
        }
    }
}
