//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IOrderPackageDao.cs
//创 建 人：罗海明
//创建时间：2014/12/16 12:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  

using System.Collections.Generic;
using Com.Panduo.Entity.Order;


namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public interface IPackageDao:IBaseDao<PackagePo,int>
    {
        /// <summary>
        /// 根据订单Id获取包裹列表
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        IList<PackagePo> GetOrderPackageByOrderId(int orderId);
    } 
}
   