//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IPackageDetailDao.cs
//创 建 人：罗海明
//创建时间：2015/02/10 13:59:50 
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
using Com.Panduo.Service;
using Com.Panduo.Service.Order;

namespace Com.Panduo.ServiceImpl.Order.Dao
{ 
    public interface IPackageDetailDao:IBaseDao<PackageDetailPo,int>
    {
        /// <summary>
        /// 搜索订单包裹列表
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="searchCriteria">查询条件</param>
        /// <param name="sorterCriteria">排序条件</param>
        /// <returns>包含分页的订单包裹列表</returns>
        PageData<PackageDetail> FindOrderPackages(int currentPage, int pageSize, IDictionary<PackageSearchCriteria, object> searchCriteria, IList<Sorter<PackageSorterCriteria>> sorterCriteria);
 
    } 
}
   