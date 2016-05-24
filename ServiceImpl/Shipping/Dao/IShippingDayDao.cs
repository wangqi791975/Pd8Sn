//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IShippingDayDao.cs
//创 建 人：罗海明
//创建时间：2015/01/29 17:59:50 
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

namespace Com.Panduo.ServiceImpl.Shipping.Dao
{ 
    public interface IShippingDayDao:IBaseDao<ShippingDayPo,int>
    {
        /// <summary>
        /// 根据运送方式Id和国家二级编码获取ShippingDay信息
        /// </summary>
        /// <param name="shippingId">运送方式Id</param>
        /// <param name="countryIsoCode2">国家二级编码</param>
        /// <returns></returns>
        ShippingDayPo GetShippingDay(int shippingId, string countryIsoCode2);
    } 
}
   