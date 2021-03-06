//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：PackageDao.cs
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
using Com.Panduo.Entity.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{
    public class PackageDao : BaseDao<PackagePo, int>, IPackageDao
    {
        public IList<PackagePo> GetOrderPackageByOrderId(int orderId)
        {
            return FindDataByHql("from PackagePo where OrderId=?", orderId);
        }
    } 
}
   