//-------------------------------------------------------------------
//版权所有：杭州潘朵信息科技有限公司 版权所有©2005-2014
//系统名称：8Seasons
//文件名称：UnitDao.cs
//创 建 人：罗海明
//创建时间：2015/01/08 17:00:40 
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
    public class UnitDao : BaseDao<UnitPo, int>, IUnitDao
    {

        public UnitPo GetUnitByUnitId(int unitId)
        {
            return GetOneObject("from UnitPo where UnitId = ?", unitId);
        }

        public IList<UnitPo> GetAllUnit()
        {
            return FindDataByHql("from UnitPo");
        }
    } 
}
   