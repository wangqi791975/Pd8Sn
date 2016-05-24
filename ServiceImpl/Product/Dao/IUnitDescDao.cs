//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8seasons
//文件名称：IUnitDescDao.cs
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
    public interface IUnitDescDao : IBaseDao<UnitDescPo, int>
    {
        /// <summary>
        /// 根据单位Id获取所有产品单位多语种信息
        /// </summary>
        /// <param name="unitId">单位Id</param>
        /// <returns>UnitDescPo列表</returns>
        IList<UnitDescPo> GetAllUnitDescByUnitId(int unitId);

        /// <summary>
        /// 根据单位Id和语种id，获取单个产品单位多语种信息
        /// </summary>
        /// <param name="unitId">单位Id</param>
        /// <param name="languageId">语种Id</param>
        /// <returns>UnitDescPo</returns>
        UnitDescPo GetUnitDesc(int unitId,int languageId);

        /// <summary>
        /// 根据单位ID列表和语种ID，批量获取产品单位
        /// </summary>
        /// <param name="unitIds"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        IList<UnitDescPo> GetAllUnitDescByUnitIdAndLangId(string unitIds, int languageId);
    } 
}
   