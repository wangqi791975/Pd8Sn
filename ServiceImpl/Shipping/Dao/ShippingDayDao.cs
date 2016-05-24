//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：ShippingDayDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 18:08:40 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using Com.Panduo.Entity.Shipping;

namespace Com.Panduo.ServiceImpl.Shipping.Dao
{ 
    public class ShippingDayDao:BaseDao<ShippingDayPo,int>, IShippingDayDao
    {
        public ShippingDayPo GetShippingDay(int shippingId, string countryIsoCode2)
        {
            return GetOneObject("from ShippingDayPo where ShippingId = ? And CountryIsoCode2=?", new object[] { shippingId, countryIsoCode2 });
        }
    } 
}
   