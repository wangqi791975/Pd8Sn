//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IUnitDao.cs
//创 建 人：罗海明
//创建时间：2015/01/08 16:49:50 
//功能说明：
//-----------------------------------------------------------------
//修改记录： 
//修改人：   
//修改时间： 
//修改内容： 
//-----------------------------------------------------------------  
using System.Collections.Generic;
using Com.Panduo.Entity.Product;

namespace Com.Panduo.ServiceImpl.Product.Dao
{
    public interface IUnitDao : IBaseDao<UnitPo, int>
    {
        /// <summary>
        /// 根据单位Id获取产品单位信息
        /// </summary>
        /// <param name="unitId">单位Id</param>
        /// <returns>UnitPo</returns>
        UnitPo GetUnitByUnitId(int unitId);

        /// <summary>
        /// 获取所有产品单位信息
        /// </summary>
        /// <returns>UnitPo列表</returns>
        IList<UnitPo> GetAllUnit();
    } 
}
   